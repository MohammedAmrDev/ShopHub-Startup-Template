# ASP.NET Core MVC Views — Complete Explanation

## Table of Contents
1. [What are Views in ASP.NET Core MVC?](#1-what-are-views-in-aspnet-core-mvc)
2. [Razor Syntax](#2-razor-syntax)
3. [View Folder Structure Convention](#3-view-folder-structure-convention)
4. [Types of Views](#4-types-of-views)
5. [ViewStart & ViewImports](#5-viewstart--viewimports)
6. [Passing Data to Views](#6-passing-data-to-views)
7. [Layouts (Shared Layouts)](#7-layouts-shared-layouts)
8. [Partial Views](#8-partial-views)
9. [View Sections](#9-view-sections)
10. [Tag Helpers](#10-tag-helpers)
11. [This Project's View Architecture](#11-this-projects-view-architecture)
12. [Complete File-by-File Breakdown](#12-complete-file-by-file-breakdown)

---

## 1. What are Views in ASP.NET Core MVC?

In the **MVC (Model-View-Controller)** pattern, **Views** are the **presentation layer** — they handle the *user interface* and *user experience*. 

- **Controllers** handle requests, process data, and decide **which view to show**.
- **Views** contain the **HTML markup + Razor code** that gets rendered into HTML and sent to the browser.
- Views are `.cshtml` files (C# + HTML = Razor).

**How a Request Flows:**
```
Browser Request → Controller Action → (fetches Model data) → View (.cshtml) → HTML → Browser
```

---

## 2. Razor Syntax

Razor is the markup syntax that lets you embed C# code inside HTML. It uses the `@` character as a transition.

### Key Razor constructs:

| Syntax | Example | Purpose |
|--------|---------|---------|
| `@code` | `@DateTime.Now` | Inline expression |
| `@{ ... }` | `@{ var x = 5; }` | Code block |
| `@if (...) { ... }` | `@if (user.IsAdmin) { <p>Admin</p> }` | Conditional |
| `@foreach (var item in Model)` | Loop through collections |
| `@Html.Raw(...)` | `@Html.Raw("<b>bold</b>")` | Render raw HTML (unescaped) |
| `@await ...` | `@await Html.PartialAsync(...)` | Async HTML helpers |

### Strongly-Typed Views

Views use the `@model` directive to specify the model type they receive from the controller:

```razor
@model IEnumerable<Category>
@model LoginViewModel
@model ProductViewModel
```

---

## 3. View Folder Structure Convention

ASP.NET Core MVC follows a **convention-based** folder structure:

```
Views/
  _ViewImports.cshtml        ← Global imports for ALL views
  _ViewStart.cshtml          ← Global code that runs before EVERY view

  ControllerName1/           ← Views for ControllerName1Controller
    Action1.cshtml           ← View for the Action1() method
    Action2.cshtml
    ...

  ControllerName2/
    Index.cshtml
    ...

  Shared/                    ← Shared layouts, partials, errors
    _Layout.cshtml
    _LoginPartial.cshtml
    Error.cshtml
```

**Rule:** Each Controller has a folder inside `Views/` with the same name (minus the "Controller" suffix). Each Action method maps to a `.cshtml` file with the matching name.

---

## 4. Types of Views

### A. **Regular Views** (Action-Specific)
These are tied to a specific controller action. The controller returns them with `return View()`:
- `Home/Index.cshtml` ← returned by `HomeController.Index()`
- `Account/Login.cshtml` ← returned by `AccountController.Login()`

### B. **Layout Views** (`_Layout.cshtml`)
The master page / shell of your application. Contains the `<html>`, `<head>`, `<body>`, navigation, footer. Other views render inside it via `@RenderBody()`.

### C. **Partial Views** (`_PartialName.cshtml`)
Reusable snippets of HTML rendered inside other views using:
```razor
<partial name="_LoginPartial" />
@await Html.PartialAsync("_ValidationScriptsPartial")
@await Html.RenderPartialAsync("_ValidationScriptsPartial")
```

### D. **ViewStart & ViewImports** (Special Files)
Run automatically for every view in the folder (and subfolders).

---

## 5. ViewStart & ViewImports

### `_ViewStart.cshtml`
Executes **before every view** in the same folder and subfolders. Typically used to set the default Layout:

```razor
@{
    Layout = "_Layout";
}
```

Views can **override** this by setting `Layout = null` or a different layout inside the view.

### `_ViewImports.cshtml`
Makes `@using` directives and `@addTagHelper` available in **all** views in the folder and subfolders — no need to repeat them in every view file.

```razor
@using myshop.Web
@using myshop.Models.ViewModels
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
```

**In this project:**
- `_ViewStart.cshtml` sets the default layout to `_Layout.cshtml`.
- `_ViewImports.cshtml` imports namespaces (`myshop.Web`, `myshop.Models.Entities`, `myshop.Models.ViewModels`, etc.) and enables Tag Helpers globally.

---

## 6. Passing Data to Views

There are three main ways controllers pass data to views:

### A. **Strongly-Typed Model** (Most Common)
```csharp
// Controller
public IActionResult Index()
{
    var categories = _unitOfWork.Category.GetAll();
    return View(categories); // model = categories
}
```
```razor
@* View *@
@model IEnumerable<Category>
@foreach (var item in Model) { ... }
```

### B. **ViewData / ViewBag** (Dynamic, untyped)
```csharp
ViewData["Title"] = "Home Page";
ViewBag.PageTitle = "Category Management";
ViewBag.CardTitle = "Create Category";
```
```razor
<h1>@ViewData["Title"]</h1>
<h2>@ViewBag.PageTitle</h2>
```

### C. **TempData** (Survives one redirect)
Used for notifications across redirects:
```csharp
TempData["Create"] = "Category created successfully!";
```
```razor
@if (TempData["Create"] != null) { <script>toastr.success(...)</script> }
```

---

## 7. Layouts (Shared Layouts)

A **Layout** provides the common HTML structure for your app. Think of it as a "master page."

### How Layouts Work:

```razor
@* _Layout.cshtml *@
<html>
<head>
    <title>@ViewData["Title"] - MyApp</title>
</head>
<body>
    <nav> ... </nav>
    <main>
        @RenderBody()   @* The specific view's content goes here *@
    </main>
    <footer> ... </footer>
    
    @await RenderSectionAsync("Scripts", required: false)   @* Optional section *@
</body>
</html>
```

### Setting a Layout:
1. **Globally:** in `_ViewStart.cshtml`: `Layout = "_Layout";`
2. **Per-View:** in the view file: `Layout = "~/Views/Shared/_Dashboard.cshtml";`
3. **No Layout:** `Layout = null;`

### Multiple Layouts in This Project:
| Layout | File | Purpose |
|--------|------|---------|
| `_Layout.cshtml` | `Views/Shared/_Layout.cshtml` | Default — used by Account, Home, UserManagement |
| `_Dashboard.cshtml` | `Views/Shared/_Dashboard.cshtml` | Admin Panel — used by Product views |
| `_Customer.cshtml` | `Views/Shared/_Customer.cshtml` | Customer Storefront — has hero banner + cart button |

---

## 8. Partial Views

Partial views are reusable fragments of markup. They're not full pages — just HTML snippets.

### Rendering Partials:
```razor
<partial name="_LoginPartial" />
@await Html.PartialAsync("_ValidationScriptsPartial")
@await Html.RenderPartialAsync("_LoginPartial")
```

### Partial views in this project:
| Partial | Purpose |
|---------|---------|
| `_LoginPartial.cshtml` | Login/Register/Logout buttons + user greeting |
| `_Toaster.cshtml` | Toastr success/error notifications from TempData |
| `_ValidationScriptsPartial.cshtml` | jQuery Validation + Unobtrusive validation scripts |

---

## 9. View Sections

**Sections** allow a view to inject content into specific parts of the layout. 

### Defining a Section in a View:
```razor
@section Scripts {
    <script src="~/js/products.js"></script>
    @await Html.PartialAsync("_ValidationScriptsPartial")
}
```

### Rendering a Section in the Layout:
```razor
@await RenderSectionAsync("Scripts", required: false)
```

`required: false` means the section is optional — the layout works even if no view defines it.

---

## 10. Tag Helpers

Tag Helpers enable **server-side C# code** to participate in creating and rendering HTML elements.

| Tag Helper | Example | Generates |
|------------|---------|-----------|
| `asp-controller` / `asp-action` | `<a asp-controller="Home" asp-action="Index">` | `<a href="/Home/Index">` |
| `asp-for` | `<input asp-for="Email" />` | `<input id="Email" name="Email" ...>` |
| `asp-validation-for` | `<span asp-validation-for="Email">` | Client-side validation span |
| `asp-validation-summary` | `<div asp-validation-summary="ModelOnly">` | Validation error summary |
| `asp-route-id` | `<a asp-action="Edit" asp-route-id="@item.Id">` | `<a href="/Category/Edit/5">` |
| `asp-items` | `<select asp-items="Model.CategoryList">` | `<option>` list from `SelectListItem` |

All Tag Helpers are enabled globally in `_ViewImports.cshtml`:
```razor
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
```

---

## 11. This Project's View Architecture

### Project Structure:
```
Views/
├── _ViewImports.cshtml          ← Global @using and Tag Helpers
├── _ViewStart.cshtml            ← Default layout = "_Layout"
│
├── Account/                     ← AccountController views
│   ├── Login.cshtml             ← Login form (LoginViewModel)
│   ├── Register.cshtml          ← Registration form (RegistrationViewModel)
│   └── ConfirmationMessage.cshtml ← Email confirmation prompt
│
├── Category/                    ← Admin CategoryController views
│   ├── Index.cshtml             ← List all categories (IEnumerable<Category>)
│   ├── Create.cshtml            ← Create category form (Category)
│   ├── Edit.cshtml              ← Edit category form (Category)
│   └── Delete.cshtml            ← Delete confirmation (Category)
│
├── Home/                        ← HomeController views
│   ├── Index.cshtml             ← Home page (public)
│   └── Privacy.cshtml           ← Privacy policy
│
├── Product/                     ← Admin ProductController views
│   ├── Index.cshtml             ← DataTable list (layout: _Dashboard)
│   ├── Create.cshtml            ← Create product form (layout: _Dashboard)
│   └── Edit.cshtml              ← Edit product form (layout: _Dashboard)
│
├── Shared/                      ← Shared layouts and partials
│   ├── _Layout.cshtml           ← Default layout (Bootstrap 5)
│   ├── _Dashboard.cshtml        ← Admin dashboard layout (AdminLTE 3)
│   ├── _Customer.cshtml         ← Customer storefront layout (hero + cart)
│   ├── _LoginPartial.cshtml     ← Login/Logout UI
│   ├── _Toaster.cshtml          ← Toastr notifications
│   ├── _ValidationScriptsPartial.cshtml ← jQuery validation scripts
│   └── Error.cshtml             ← Error page (ErrorViewModel)
│
└── UserManagement/              ← UserManagementController views
    └── Index.cshtml             ← User management table + pagination
```

### Layout Flow (Which Layout Each View Uses):

```
_ViewStart.cshtml  →  Layout = "_Layout"
                         │
                         ├── Account/*        →  _Layout.cshtml (default)
                         ├── Home/*           →  _Layout.cshtml (default)
                         ├── UserManagement/  →  _Layout.cshtml (default)
                         │
                         ├── Category/*       →  _Layout.cshtml (default, but ViewBag.PageTitle/CardTitle)
                         │                      Actually renders inside _Layout.cshtml but uses custom card styles
                         │
                         └── Product/*        →  _Dashboard.cshtml (explicitly set)
```

**Key Insight:** Product views explicitly set `Layout = "~/Views/Shared/_Dashboard.cshtml"`, overriding `_ViewStart.cshtml`. Category views rely on the default layout but use `ViewBag.PageTitle` and `ViewBag.CardTitle` which are designed for the Dashboard card pattern.

---

## 12. Complete File-by-File Breakdown

### A. Special Files

#### `_ViewImports.cshtml`
```razor
@using myshop.Web
@using myshop.Models.Entities
@using myshop.Entities.Models
@using myshop.Models.ViewModels
@using Microsoft.AspNetCore.Http
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
```
Imports all commonly used namespaces and enables Tag Helpers globally.

#### `_ViewStart.cshtml`
```razor
@{
    Layout = "_Layout";
}
```
Sets default layout to `_Layout.cshtml` for all views unless overridden.

---

### B. `Views/Shared/` — Shared Resources

#### `_Layout.cshtml` (Default Layout)
- **Used by:** Account, Home, UserManagement, Category views
- **Tech Stack:** Bootstrap 5, custom CSS
- **Features:**
  - Gradient background with fade-in animation
  - Styled navbar with animated slide-down
  - Navigation links (Products, Categories) shown only when user is signed in
  - `_LoginPartial` included in navbar
  - `@RenderBody()` in a container
  - Footer with copyright
  - jQuery + Bootstrap JS + site.js
  - `RenderSectionAsync("Scripts", required: false)` for page-specific scripts

#### `_Dashboard.cshtml` (Admin Layout)
- **Used by:** Product views (explicitly)
- **Tech Stack:** AdminLTE 3, Font Awesome, Poppins font
- **Features:**
  - Sidebar with navigation: Dashboard, Categories, Products, Users, Orders
  - Top navbar with hamburger menu
  - Card wrapper with `@ViewBag.PageTitle` and `@ViewBag.CardTitle`
  - Includes `_Toaster` partial for notifications
  - DataTables, Toastr, SweetAlert2 CDN includes
  - `@RenderBody()` inside card-body
  - jQuery UI, AdminLTE JS

#### `_Customer.cshtml` (Customer Storefront)
- **Used by:** Potentially for customer-facing product browsing (partial code commented out)
- **Tech Stack:** Bootstrap 5, Bootstrap Icons
- **Features:**
  - Hero banner with welcome message
  - Cart button with badge (count from session/DB)
  - `_LoginPartial` in a styled box
  - Product-focused navigation (most links commented out)
  - Gradient footer

#### `_LoginPartial.cshtml`
- Shows **Logout** button + "Hello username!" when signed in
- Shows **Register** and **Login** buttons when not signed in
- Uses `SignInManager<ApplicationUser>` and `UserManager<ApplicationUser>` injected via `@inject`

#### `_Toaster.cshtml`
- Renders Toastr notifications based on `TempData`:
  - `TempData["Create"]` → toastr.success (green)
  - `TempData["Update"]` → toastr.info (blue)
  - `TempData["Delete"]` → toastr.error (red)

#### `_ValidationScriptsPartial.cshtml`
```razor
<script src="~/lib/jquery-validation/dist/jquery.validate.min.js"></script>
<script src="~/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js"></script>
```
Includes jQuery Validate + Unobtrusive for client-side validation.

#### `Error.cshtml`
- Model: `ErrorViewModel`
- Shows error message and Request ID when in production
- Recommends Development mode for detailed errors

---

### C. `Views/Account/` — Authentication

All use default `_Layout.cshtml`.

#### `Login.cshtml`
- **Model:** `LoginViewModel` (Email, Password, RememberMe)
- **Purpose:** Sign-in form
- **Features:**
  - Centered card with Bootstrap styling
  - Email + Password inputs with validation
  - Remember Me checkbox
  - Forgot Password link
  - Link to Register page
  - `_ValidationScriptsPartial` in Scripts section

#### `Register.cshtml`
- **Model:** `RegistrationViewModel` (FirstName, LastName, Email, Password, PasswordConfirm, Role)
- **Purpose:** New user registration
- **Features:**
  - First/Last name side by side
  - Email, Password, Confirm Password
  - Hidden Role field (for customer/admin toggle — commented out)
  - `_ValidationScriptsPartial` in Scripts section

#### `ConfirmationMessage.cshtml`
- **Model:** None (simple static page)
- **Purpose:** Tells user to check their email for confirmation link

---

### D. `Views/Category/` — Admin CRUD

All use default `_Layout.cshtml` with `ViewBag.PageTitle` and `ViewBag.CardTitle`. These views use a custom `.custom-card` CSS pattern.

#### `Index.cshtml`
- **Model:** `IEnumerable<Category>`
- **Purpose:** List all categories
- **Features:**
  - Custom styled table with blue headers
  - Create button → `/Category/Create`
  - Each row: Edit (green) and Delete (red) buttons
  - Shows Name, Description (rendered as HTML), Created Date

#### `Create.cshtml`
- **Model:** `Category`
- **Purpose:** Create a new category
- **Features:**
  - Form with Name + Description inputs
  - Back button + Create button
  - Custom card styling
  - `_ValidationScriptsPartial` in Scripts section

#### `Edit.cshtml`
- **Model:** `Category`
- **Purpose:** Edit existing category
- **Features:**
  - Hidden `Id` field
  - Pre-filled Name + Description
  - Description text has HTML tag stripping
  - Save Changes button + Back button
  - `_ValidationScriptsPartial` in Scripts section

#### `Delete.cshtml`
- **Model:** `Category`
- **Purpose:** Delete confirmation
- **Features:**
  - Disabled inputs showing current values
  - Form posts to `DeleteCategory` action
  - Back button + Delete button
  - `_ValidationScriptsPartial` in Scripts section

---

### E. `Views/Home/` — Public Pages

Both use default `_Layout.cshtml`.

#### `Index.cshtml`
- Simple welcome page with link to ASP.NET Core docs
- Sets `ViewData["Title"] = "Home Page"`

#### `Privacy.cshtml`
- Static privacy policy placeholder
- Sets `ViewData["Title"] = "Privacy Policy"`

---

### F. `Views/Product/` — Admin CRUD (Dashboard Layout)

These explicitly set `Layout = "~/Views/Shared/_Dashboard.cshtml"` and use the AdminLTE dashboard theme.

#### `Index.cshtml`
- **Layout:** `_Dashboard.cshtml`
- **Purpose:** List all products via DataTables (AJAX)
- **Features:**
  - DataTable (`#mytable`) loads data from `/Product/GetData` JSON endpoint
  - Server-side filtering/sorting via DataTables
  - "Create Product" button
  - Custom `products.js` script for DataTable initialization
  - Columns: Name, Description, Price, Category, Actions

#### `Create.cshtml`
- **Model:** `ProductViewModel`
- **Layout:** `_Dashboard.cshtml`
- **Purpose:** Create a new product with image upload
- **Features:**
  - File upload with image preview (via JavaScript `fillimg()`)
  - Category dropdown (`asp-items="Model.CategoryList"`)
  - `enctype="multipart/form-data"` on form (required for file uploads)
  - Name, Description, Price inputs
  - Image preview element
  - `_ValidationScriptsPartial` + custom JS

#### `Edit.cshtml`
- **Model:** `ProductViewModel`
- **Layout:** `_Dashboard.cshtml`
- **Purpose:** Edit existing product
- **Features:**
  - Hidden fields for `Id` and `ImageURL`
  - File upload for replacing image
  - Shows current image preview via `~/@Model.ImageURL`
  - Category dropdown
  - `_ValidationScriptsPartial` + custom JS for image preview

---

### G. `Views/UserManagement/` — Admin User Management

Uses default `_Layout.cshtml`.

#### `Index.cshtml`
- **Model:** `IEnumerable<UserManagementViewModel>`
- **Purpose:** Manage system users (Admin only)
- **Features:**
  - Search input with server-side filtering
  - Table with: Username, Email, Role badge, Lock status badge (green=Active, red=Locked)
  - Action buttons per row (inline forms):
    - **ToggleRole:** Switches between Customer/Admin
    - **ToggleLock:** Locks/unlocks user account
    - **Delete:** With JavaScript `confirm()` dialog
  - Manual pagination with Previous/Next + page numbers
  - Uses `ViewBag.PageIndex`, `ViewBag.PagesNum`, `ViewBag.SearchBy`

---

## Summary of Data Flow (How Everything Connects)

```
                     ┌────────────────────────────────────────┐
                     │           Program.cs                    │
                     │  services.AddControllersWithViews()     │
                     │  app.MapControllerRoute(                │
                     │    "{controller=Home}/{action=Index}")  │
                     └──────────────┬─────────────────────────┘
                                    │
          HTTP Request: GET /Category/Create/5
                                    │
                     ┌──────────────▼─────────────────────────┐
                     │       CategoryController.Create(5)      │
                     │  [Authorize(Roles = "Admin")]           │
                     │  var category = _unitOfWork.Category    │
                     │                    .GetById(5);         │
                     │  return View(category);                 │
                     └──────────────┬─────────────────────────┘
                                    │
                     ┌──────────────▼─────────────────────────┐
                     │  _ViewStart.cshtml                      │
                     │  Layout = "_Layout"  (unless overridden)│
                     └──────────────┬─────────────────────────┘
                                    │
                     ┌──────────────▼─────────────────────────┐
                     │  Views/Category/Create.cshtml            │
                     │  @model Category                        │
                     │  ViewBag.PageTitle / ViewBag.CardTitle   │
                     │  Form with asp-for tag helpers           │
                     └──────────────┬─────────────────────────┘
                                    │
                     ┌──────────────▼─────────────────────────┐
                     │  Views/Shared/_Layout.cshtml            │
                     │  @RenderBody() ← Category/Create.cshtml │
                     │  @await RenderSectionAsync("Scripts")   │
                     └──────────────┬─────────────────────────┘
                                    │
                     ┌──────────────▼─────────────────────────┐
                     │         Full HTML Response              │
                     │   Layout + View combined into one page  │
                     └─────────────────────────────────────────┘
```

---

## Key ASP.NET Conventions Used in This Project

| Convention | How It's Used |
|------------|---------------|
| **Controller → View mapping** | `CategoryController.Create()` → `/Views/Category/Create.cshtml` |
| **`_ViewStart.cshtml`** | Sets default `_Layout.cshtml` for all views |
| **`_ViewImports.cshtml`** | Global `@using` statements + Tag Helpers |
| **`Shared/` folder** | Layouts, partials, error page |
| **ViewBag / ViewData** | Passing page title, card title, pagination state |
| **ViewModel pattern** | `LoginViewModel`, `RegistrationViewModel`, `ProductViewModel`, `UserManagementViewModel` |
| **Sections** | `@section Scripts { ... }` for page-specific JS |
| **Partial views** | `_LoginPartial`, `_Toaster`, `_ValidationScriptsPartial` |
| **Layout override** | Product views explicitly set `Layout = "~/Views/Shared/_Dashboard.cshtml"` |
| **Model binding** | Form inputs automatically bind to model properties via `asp-for` |
| **Client-side validation** | `_ValidationScriptsPartial` + unobtrusive jQuery validation |
| **Server-side validation** | `asp-validation-summary` / `asp-validation-for` display `ModelState` errors |
