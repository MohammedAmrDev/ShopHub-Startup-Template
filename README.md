# Fix Checklist
- [ ] Create a Category DTO and deal with it.
- [ ] Remove "Register as Admin" link.
- [ ] Add explicit [Authorize(Roles = "...")] attributes instead of relying solely on the global fallback policy.
## Fixing the informality in design between the admin views (product, category, usermanagement)
- [ ] Use the admin dashboard in Category and UserManagement views (to be like product)
- [ ] Implement Category and UserManagement with feature in Product
    - AdminLTE (a professional admin dashboard theme)
    - DataTables (fancy sortable/searchable tables)
    - SweetAlert2 (nice-looking popup notifications/toasts)
- [ ] Build a real customer storefront instead of the default Home/Index page


## Before starting

- Setup the connection string
- Setup the Mail SMTP (for mail confirmation) in the configuration file (Based on the MailSetting model in Models/Settings/)
