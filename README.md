# Keycloak-Demo

This demo is made with the help from this guide: https://medium.com/@xavier.hahn/adding-authorization-to-asp-net-core-app-using-keycloak-c6c96ee0e655

## Keycloak setup

1. Deploy the Keycloak

```
docker-compose -f docker-compose-keycloak.yml up
```

or

```
docker stack deploy -c docker-compose-keycloak.yml keycloak-demo
```

2. Open the Keycloak `Administration Console` from http://localhost:8080/

|||
|-|-|
| Username | keycloak |
| Password | keycloak |

_As written in the docker-compose-keycloak.yml file_

3. Create a new client

|||
|-|-|
| Client ID | dotnet-demo |
| Client Protocol | openid-connect |
| Root URL | http://localhost:5161 |

4. Edit the newly created client

Change the following properties and click save
|||
|-|-|
| Access Type | Confidential |
| Implicit Flow Enabled | ON |

5. Go to the **Roles tab** (inside the `dotnet-demo` client) and create your roles

||
|-|
| Admin |
| Manager |
| Employer |

6. Go to the **Mappers** (inside the `dotnet-demo` client) and create the user client roles mapper

Change to the following values and click save
|||
|-|-|
| Protocol | openid-connect |
| Name | User client roles |
| Mapper Type | User Client Role |
| Client ID | dotnet-demo |
| Client Role prefix | |
| Multivalued | ON |
| Token Claim Name | user_roles |
| Claim JSON Type | String |
| Add to ID token | ON |
| Add to access token | ON |
| Add to userinfo | ON |

7. Go to the **Credentials** (inside the `dotnet-demo` client) and note down the `Secret` value

8. Go to the **Users** tab under **Manage** section and add role(s) for your user or create a new user

You can add roles for your user by selecting your user and then going to the `Role Mappings` tab. From there select the `dotnet-demo` in the **Client Roles** section. Then move your role(s) from **Available Roles** to **Assigned Roles**

## .NET Core setup

1. Open the `launchSettings.json` from the `Keycloak-Demo\Properties\` folder.

||||
|-|-|-|
| KEYCLOAK_ENDPOINT | http://localhost:8080/realms/master | _http://localhost:8080/auth/realms/master_ in versions before Keycloak 17.0 |
| KEYCLOAK_CLIENT_ID | dotnet-demo | _The name of the client you created_ |
| KEYCLOAK_CLIENT_SECRET | m383YWl9QiOBOrgiM80xdzXyQK0JYWme | _The secret from the step 7. in Keycloak Setup_ |

2. Run the Keycloak-Demo project (make sure you are running the `Keycloak_Demo` profile and not for example IIS Express)

3. Check that the login works by going to http://localhost:5161/info/authenticated

If you are not already logged in to Keycloak, you should be redirected to Keycloak login page. If you are logged in/after login, you should see JSON data with "User has authenticated" and the user id.

4. Check that the roles work

Depending on the roles you assigned, the following URLs will either redirect you to the AccessDenied page (not implemented, so an empty page) or show that you have the role.

||||
|-|-|-|
| AdminPolicy | http://localhost:5161/info/adminOnly | Checks for the Admin role with the `AdminOnly` policy |
| Admin | http://localhost:5161/info/admin | Checks for the Admin role |
| Manager | http://localhost:5161/info/manager | Checks for the Manager role |
| Employer | http://localhost:5161/info/employer | Checks for the Employer role |
