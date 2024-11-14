
users => (GET) ✔️
  current => (GET)  ✔️
    spotlights => (GET) ✔️
      {id} => (DELETE) - Dismiss ✔️
    notifications => (GET) ✔️
      {id}
        read => (POST) - Mark as read ✔️
      read => (POST) - Mark all as read ✔️
    events => (GET) ✔️
    ---------------------------
  projects => (GET)
    {user_projects_id} => (GET, PATCH)
        git  => (GET, PUT)
        reviews  => (GET, POST)
        invitation  => (PUT, DELETE)
    ---------------------------
  goals => (GET)
    {user_goals_id} => (GET, PATCH)
        projects => (GET)
    ---------------------------

  cursus => (GET)
    {user_cursus_id} => (GET, PATCH)
        projects
        goals
    ---------------------------

  {id} => (GET, PATCH, DELETE) <- Delete makes anon
    details - (GET, PUT)
    bio - (GET) - To avoid the size of the JSON response
    projects - (GET)
      {project_id}
        (GET): ... -> Returns the project instance
        (POST): ... -> "Subscribe" to a project
        (DELETE): ... -> "Unsubscribe" from a project -> Invalidates all evals
        git => (GET)
        ---------------------------
    goals => (GET)
      {goal_id}
        (GET): ... -> Returns the goal instance
        (POST): ... -> "Subscribe" to a goal
        (DELETE): ... -> "Unsubscribe" from a goal -> Halts all projects
        ---------------------------
    cursus => (GET)
      {cursus_id}
        (GET): ... -> Returns the cursus instance
        (POST): ... -> "Subscribe" to a cursus
        (DELETE): ... -> "Unsubscribe" from a cursus -> Halts everything!
        ---------------------------

projects => (GET, POST)
  {id} => (GET, PATCH, DELETE) <- Delete deprecate, not hard

  ---------------------------
cursus => (GET, POST)
  {id} => (GET, PATCH, DELETE) <- Delete deprecate, not hard
  ---------------------------
goals => (GET, POST)
  {id} => (GET, PATCH, DELETE) <- Delete deprecate, not hard
  ---------------------------
spotlights => (GET, POST)
  {id} => (GET, PATCH, DELETE)
  ---------------------------
events => (GET, POST)
  {id} => (GET, PATCH, DELETE)
    ---------------------------
notifications => (GET, POST)
  {id} => (GET, PATCH, DELETE)
    ---------------------------
rubric => (GET, POST)
  {id} => (GET, PATCH, DELETE) <- Delete deprecate, not hard
    ---------------------------
feedback => (GET, POST)
  {id} => (GET, PATCH, DELETE)
    ---------------------------
feature => (GET, POST)
  {id} => (GET, PATCH, DELETE)

---

.
├── users => (GET)
│   ├── current => (GET)
│   │   ├── spotlights => (GET)
│   │   │   └── {id} => (DELETE) - Dismiss
│   │   ├── notifications => (GET)
│   │   │   ├── {id}
│   │   │   │   └── read => (POST) - Mark as read
│   │   │   └── read => (POST) - Mark all as read
│   │   ├── events => (GET)
│   │   └── ---------------------------
│   ├── projects => (GET)
│   │   ├── {user_projects_id} => (GET, PATCH)
│   │   │   ├── git  => (GET, PUT)
│   │   │   ├── reviews  => (GET, POST)
│   │   │   └── invitation  => (PUT, DELETE) - Send / accept / decline
│   │   └── ---------------------------
│   ├── goals => (GET)
│   │   ├── {user_goals_id} => (GET, PATCH)
│   │   │   └── projects => (GET)
│   │   └── ---------------------------
│   ├── cursus => (GET)
│   │   ├── {user_cursus_id} => (GET, PATCH)
│   │   │   ├── projects
│   │   │   └── goals
│   │   └── ---------------------------
│   └── {id} => (GET, PATCH, DELETE) <- Delete makes anon
│       ├── details - (GET, PUT)
│       ├── projects - (GET)
│       │   └── {project_id}
│       │       ├── (GET): ... -> Returns the project instance
│       │       ├── (POST): ... -> "Subscribe" to a project
│       │       ├── (DELETE): ... -> "Unsubscribe" from a project -> Invalidates all evals
│       │       ├── git => (GET)
│       │       └── ---------------------------
│       ├── goals => (GET)
│       │   └── {goal_id}
│       │       ├── (GET): ... -> Returns the goal instance
│       │       ├── (POST): ... -> "Subscribe" to a goal
│       │       ├── (DELETE): ... -> "Unsubscribe" from a goal -> Halts all projects
│       │       └── ---------------------------
│       └── cursus => (GET)
│           └── {cursus_id}
│               ├── (GET): ... -> Returns the cursus instance
│               ├── (POST): ... -> "Subscribe" to a cursus
│               ├── (DELETE): ... -> "Unsubscribe" from a cursus -> Halts everything!
│               └── ---------------------------
├── projects => (GET, POST)
│   ├── {id} => (GET, PATCH, DELETE) <- Delete deprecate, not hard
│   └── ---------------------------
├── cursus => (GET, POST)
│   ├── {id} => (GET, PATCH, DELETE) <- Delete deprecate, not hard
│   └── ---------------------------
├── goals => (GET, POST)
│   ├── {id} => (GET, PATCH, DELETE) <- Delete deprecate, not hard
│   └── ---------------------------
├── spotlights => (GET, POST)
│   ├── {id} => (GET, PATCH, DELETE)
│   └── ---------------------------
├── events => (GET, POST)
│   └── {id} => (GET, PATCH, DELETE)
│       └── ---------------------------
├── notifications => (GET, POST)
│   └── {id} => (GET, PATCH, DELETE)
│       └── ---------------------------
├── rubric => (GET, POST)
│   └── {id} => (GET, PATCH, DELETE) <- Delete deprecate, not hard
│       └── ---------------------------
├── feedback => (GET, POST)
│   └── {id} => (GET, PATCH, DELETE)
│       └── ---------------------------
└── feature => (GET, POST)
    └── {id} => (GET, PATCH, DELETE)****
