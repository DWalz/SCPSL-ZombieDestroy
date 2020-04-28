# SCPSL-ZombieDestroy

Zombies feeling underwhelming and having too little impact on games? They now can destroy
doors as a group - even those that usually have to be opened by keycards. This makes
experience as SCP 049-2 a little more exciting than just running around and right-clicking.

Built on the EXILED framework for SCP: Secret Laboratory

Download the compiled `SCPSL-ZombieAcces.dll` in the [releases](https://github.com/DWalz/SCPSL-ZombieDestroy/releases) tab.
If you want to compile it yourself, download the source code and do so.


## Setup

Setup your EXILED-Server (either follow the installation guide on the EXILED repository or use the GUI Installer for Windows)

Open the `%appdata%` folder (UNIX: `~/.config`) and add the `SCPSL-ZombieDestroy.dll` into the `Plugins` folder.

Restart the server and have fun

🔗 Links:

 - Exiled: https://github.com/galaxy119/EXILED
 - Exiled Installer for Windows: https://github.com/RogerFK/EXILED-Windows-Installer
 
 
 ## Configuration
 
 You can config all plugins by adding the follwing key-value pairs to the server 
 configuration file (found at `%appdata%\EXILED\your-port.yml`).
 
| Config Key                     | Default Value | Possible Values  |
|--------------------------------|---------------|------------------|
| `zd_enabled`                   | `true`        | `true` / `false` |
| `zd_two_zombies_limit`         | `6`           | `0`, `1`, ...    |
| `zd_allow_unbreakable_destroy` | `true`        | `true` / `false` |

**`zd_enabled`**: If the plugin is enabled. If set to `false`, 
the event listeners and handlers wont be registered.

**`zd_two_zombies_limit`**: There are either `2` or `3` zombies required to force
open doors. This number indicated the threshold, the maximum number of players in the game
where there are still only `2` zombies required to do so.

**`zd_allow_unbreakable_destroy`**: This value indiates if zombies are allowed to force open
doors that usually cannot be exploded by grenades. Examples of such doors are the gate doors
or the 914-door. These doors do not explode like others doo so they disappear if they are destroyed
which can lead to graphics issues and such.


### Example config

```yaml
zd_enabled: true
zd_two_zombies_limit: 6
zd_allow_unbreakable_destroy: true
```
