# Musync API — Refactor & Improvement Plan

Generated from a full-codebase analysis (architecture, security, testing, data layer, API contract stability, dead code) on 2026-08-27. Findings are grouped by topic so each group can become its own branch/PR. Check items off as they're completed.

Urgency legend: 🔴 Blocking (real bug or exposure) · 🟡 Nice-to-have (matters as the app grows) · ⚪ Can wait (cosmetic/deferred).

---

## Group 1 — Security: Critical exposure 🔴 (recommend its own branch, first)

- [x] **JWT signing key committed in plaintext**, in git history (`Musync.Api/appsettings.json:12`, added in commit `2c76fb8`). Anyone with repo access can forge tokens for any user ID, including future accounts. Fix: move to user-secrets/env var/secret manager, rotate the key. Rotating doesn't erase it from git history — decide separately whether that matters for you (private repo vs. not).
- [x] **`POST /api/posts` has no `[Authorize]`**, and `CreatePostCommand.AuthorId` is taken directly from the client body with no check against the caller (`PostController.cs:24-31`, `CreatePostCommandHandler`). Net effect: anyone, unauthenticated, can create a post attributed to any existing user. Fix: add `[Authorize]`, derive `AuthorId` from `ICurrentUserService.CurrentUserId`, drop it from the client-facing command.
- [x] **Unmapped exceptions leak as 500s with stack traces to every client** (`DefaultExceptionHandler` puts `ex.StackTrace` in the response `Detail` for all environments, not just dev). Two concrete paths hit this instead of returning proper 401/404: `CurrentUserService.GetCurrentUserAsync()` throws a bare `Exception("Current user not found")`, and `FollowUserCommandHandler`/`UnfollowUserCommandHandler` throw `UnauthorizedAccessException` with no handler registered for it. Fix: map these to real exception types (`NotFoundException`/`UnauthorizedAccessException` handler), and stop returning `StackTrace` in responses outside Development.
- [x] **`GET /api/users` returns every user's email, unpaged, to any authenticated caller** (`UserDTO.Email` + `GetUsersQueryHandler` maps the whole table). Combined with weak signup friction (see Group 2), this is a one-call email-harvesting endpoint. Fix: paginate, and drop `Email` from the list-facing DTO (keep it only where the caller is looking at their own profile).
- [x] **File upload has no type/size validation and a path-traversal-prone filename** (`CreatePostCommandHandler.SaveImage`, `UpdateAvatarCommandHandler.SaveImage`). Client-controlled `image.FileName` is concatenated into the save path with only a GUID prefix on the first segment — embedded `/`/`..` isn't stripped. No content-type/magic-byte check means an `.svg`/`.html` upload gets served back from your own domain with a matching content-type (stored XSS vector), and there's no size cap beyond Kestrel's default (~28.6MB). Fix: sanitize filename to just an extension + generated name, whitelist image content-types/magic bytes, enforce a size limit.
- [x] **Docker/Compose hardcode `ASPNETCORE_ENVIRONMENT=Development`** for the shipped container (`Dockerfile`, `docker-compose.yml`), which means the "dev-only" Swagger UI / auto-login gating in `Program.cs` is always active wherever this image runs, and `UseHttpsRedirection()` is permanently commented out — tokens/passwords travel in cleartext with no TLS termination configured. Fix: add a real `Production` environment config for deployment, keep Development only for local `dotnet run`.

## Group 2 — Security: hardening 🟡 (can bundle with Group 1 or do after)

- [ ] CORS is fully open (`AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()`). Not classic CSRF risk since auth is bearer-token (not cookie) based, but it removes any barrier if a token ever leaks into a browser context, and lets any web page act as a full API client. Worth restricting to your actual app origins once you have a web client, but not urgent for a mobile-only backend today.
- [x] Login has no brute-force protection (`CheckPasswordSignInAsync(..., lockoutOnFailure: false)`), no rate limiting anywhere, and leaks user-enumeration info (404 for unknown email vs. 400 for wrong password).
- [ ] Refresh tokens never expire, aren't revocable (no logout/revoke endpoint), and only one is stored per user (logging in on a second device silently kills the first device's session).
- [x] Password policy is 6 chars, no complexity — reasonable to defer given this is early-stage, but worth deciding intentionally rather than by default.
- [x] `app.UseAuthentication()` is missing from `Program.cs`. Verified it's not currently broken — ASP.NET Core's authorization middleware implicitly authenticates via the sole default scheme — but it's a fragile, non-standard omission that will bite if a second auth scheme or custom `AllowAnonymous` logic is ever added. Cheap one-line fix, low risk to bundle into Group 1.

## Group 3 — Data correctness bugs 🔴 (recommend own small branch — active bugs, not future risk)

- [ ] **`FollowersCount`/`FollowedCount` are always 0** on `GET /api/users/{id}` and `GET /api/users` — `GetUserQueryHandler`/`GetUsersQueryHandler` never `.Include()` the `Followers`/`Followed` navigation properties that the mapping profile reads from. Contrast with `CurrentUserService`, which does include them correctly — so the fix is just adding the missing `.Include()` calls (or better, centralizing this query, see Group 4).
- [ ] **`PostDTO.Author` can come back null despite being `required`** for `GET /api/posts/author/{authorId}` — `PostRepository.GetPostsByAuthorIdAsync` doesn't `.Include(post => post.Author)`, unlike `GetAllAsync` which does.
- [ ] **Duplicate likes are possible under concurrent requests** — the `PostLike(UserId, PostId)` index is not unique, and the app does check-then-insert with no DB-level constraint backing it up. A rapid double-tap can create two `PostLike` rows for the same user+post. Fix: make the index unique.

## Group 4 — Data layer: scalability/growth risk 🟡 (not urgent at current data volume, but will become 🔴 as usage grows)

- [ ] **No pagination on `GetAllPosts`, `GetPostsByAuthorId`, or `GetUsers`** — each loads the entire table on every request. This is the single biggest scalability risk in the codebase; `SearchUsers` already has correct paging as a reference pattern to copy.
- [ ] Feed ordering (`OrderByDescending` in `PostRepository.GetAllAsync`) is applied after materializing the full list into memory, not pushed to SQL — becomes real overhead once paging is added if not fixed at the same time.
- [ ] `FollowUserCommandHandler`/`UnfollowUserCommandHandler` load the entire `Followed`/`Followers` collection of full `ApplicationUser` rows (including password hashes etc.) just to add/remove one relation and read a count. This scales linearly with follower count per follow/unfollow action — will get expensive for any account with a large following.
- [ ] No unit-of-work: a follow operation does two separate `UserManager.UpdateAsync` calls (two separate `SaveChanges`/transactions) for what's logically one operation — a crash between them leaves an asymmetric follow graph.
- [ ] `GetAllPostsQueryHandler` does an O(N·M) in-memory loop to mark "did I like this post" — fine now, will show up in profiling once posts/likes grow.
- [ ] Missing index on `Post.CreatedAt` (or composite `AuthorId+CreatedAt`) — moot until pagination/ordering is pushed to SQL, but should land in the same change.

## Group 5 — Architecture/CQRS pattern consistency 🟡

- [ ] **FluentValidation pattern is inconsistently applied.** Post/Like features correctly validate via a `*Validator` + `BadRequestException` per CLAUDE.md's documented pattern. Follow/Unfollow have no validator at all (inline `if`/`throw`), `UpdateAvatarCommandHandler` has no validation on the uploaded file, and `GetUserQueryHandler` throws `NotFoundException` directly instead of via a validator rule — meaning "entity not found" is a 400 in Post/Like features but a 404 in User/Follow features for the same conceptual failure. Worth picking one pattern and applying it everywhere.
- [ ] **No repository abstraction for `ApplicationUser`/Follow** — every user/follow handler reaches directly into `UserManager<ApplicationUser>.Users` and calls EF-specific LINQ (`.Include`, `.ToListAsync`) straight from the Application layer. This is the actual Clean Architecture leak in the codebase (Post/Like/Instrument/Genre correctly hide EF behind repositories); it's also the root cause of the missing-`Include` bugs in Group 3, since each handler independently decides what to include with nothing enforcing consistency.
- [ ] **Duplicated file-upload logic** between `CreatePostCommandHandler.SaveImage` and `UpdateAvatarCommandHandler.SaveImage` — same GUID+copy logic, but they disagree on returned path format (leading slash vs. not). Extract into a shared `IFileStorageService`.
- [ ] **"Load fully-populated current user" logic is duplicated four times** (`FollowUserCommandHandler`, `UnfollowUserCommandHandler`, `CurrentUserService`, `AuthService`) — a good candidate to centralize once the repository abstraction above exists.
- [ ] The `Follow` feature's file layout doesn't follow the `Features/<Area>/[Commands|Queries]/<UseCase>/` convention used everywhere else (flat files instead of nested use-case folders), and has file/class name mismatches (`FollowCommand.cs` contains `FollowUserCommand`, etc. — CLAUDE.md already flags one instance of this but there are more).

## Group 6 — API contract stability 🟡 (matters because frontend generates types from this)

- [ ] **`FollowedCount` vs `FollowingCount`** — `UserDTO`/`UserProfileDTO` call it one thing, `FollowResultDTO` calls the identical concept the other. This produces two different generated TS field names for the same value depending on endpoint — worth unifying before more DTOs copy whichever name.
- [ ] **Three different styles for the same "user card" concept**: `UserDTO` (record, mostly init-only but `IsFollowed` is mutable), `UserProfileDTO` (class, init), `UserSearchDTO` (class, fully mutable, defaults instead of `required`). Also `UserSearchDTO` is missing a following-count field the others have. Picking one shape/style convention now is cheaper than after more DTOs exist.
- [ ] **`PostDTO.Caption` is declared non-nullable but the domain `Post.Caption` is nullable**, with no null-handling in the mapping — a null caption can silently land in a field the OpenAPI schema (and generated TS type) says is always a string.
- [ ] No enum types exist yet anywhere in the codebase, so there's no live bug — but no `JsonStringEnumConverter` convention is set either, so the first enum added (e.g. a future `PostVisibility`) will default to serializing as an int, which is a classic frontend-breaking trap. Worth deciding the convention before it's needed.
- [ ] `Post.Caption` has no max length enforced anywhere (validator, entity config, or DB) — currently bounded only by ASP.NET's default multipart field size (~4MB), which is both a storage-abuse vector and contract fragility.
- [ ] `UserController.GetUser`/`GetAllUsers` return bare `ActionResult` instead of `ActionResult<T>` (everywhere else in the codebase uses the generic form) — the documented response type only comes from an attribute with no compiler link, so it can silently drift from the real return value.
- [ ] No API versioning exists — not urgent solo/early-stage, but worth being aware it doesn't yet support running two contract versions simultaneously during a mobile rollout.

## Group 7 — Dead code / incomplete scaffolding ⚪ (low risk, mostly cleanup)

- [ ] **Genre/Band feature is half-built**: full persistence layer (entities, repository, EF config, seeded data, migration) exists and is wired into DI, but there's no Application feature slice, controller, or DTO — nothing in the app actually uses it. Decide: finish it (mirrors the already-complete `FavoriteInstruments` feature) or remove the dead plumbing.
- [ ] `Band` has no repository interface at all despite being a full entity with EF config — even less finished than `Genre`.
- [ ] `ApplicationUser.Bio` and `ApplicationUser.FavoriteGenres` are declared but never read/written anywhere.
- [ ] Orphaned: `UserProfileDTO` (unreferenced), `IBaseService`/`IInstrumentService` interfaces (never implemented/registered).
- [ ] Minor leftovers: a commented-out debug line in `TokenProvider.GenerateAccessToken`, unused `using`s in `FollowResultDTO`, `internal` vs `public` inconsistency across AutoMapper profile classes.

## Group 8 — Testing (assessment, not a checklist to "add tests everywhere")

Current risk level: meaningfully high for a codebase this size. Several of the bugs above (FollowersCount always 0, missing `[Authorize]` on post creation, the duplicate-like race, the exception-mapping gap turning auth failures into 500s) are exactly the class of bug a small, targeted test suite would have caught immediately, and none of them were caught because there's currently zero automated coverage over ~15 CQRS handlers plus auth.

Prioritize by blast radius, not coverage percentage:

- [ ] **Authorization smoke tests** (highest ROI): one integration test suite (`WebApplicationFactory`) that hits every `[Authorize]`-decorated endpoint without a token and asserts 401, and every endpoint without `[Authorize]` and confirms that's intentional. This single suite would have caught the `CreatePost` bug directly.
- [ ] **`AuthService` (Login/Register/Refresh)** — unit tests with a mocked `UserManager`/`SignInManager`: this is the highest-consequence code path in the app and currently has zero coverage.
- [ ] **Exception-mapping integration test**: assert that each exception type thrown anywhere in the app maps to the expected status code, not a 500 — would have caught the `UnauthorizedAccessException`/raw-`Exception` gap.
- [ ] **Follow/Unfollow handlers** — mutate a bidirectional relationship across two separate `SaveChanges` calls with no transaction; a unit test around the "both sides updated consistently" invariant is cheap and protects against regressions if this logic is refactored (see Group 5).
- [ ] FluentValidation validators — cheapest tests to write (pure, no mocking), directly exercise the rules Group 5/6 findings show are inconsistently applied — worth doing as validators get consolidated, not as a separate effort.

Skip: CRUD-only handlers with no branching logic (e.g. `GetInstruments`) — low value for the effort until the codebase is much larger.

