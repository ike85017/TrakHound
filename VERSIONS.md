# Versions
List of what is contained in each version and what is planned for future versions.

TrakHound build version scheme is defined below:

[Major Revision].[Minor Revision].[Issue Fix]

Note: Versions of TrakHound previous to Beta v1.7.0 may have not followed this pattern exactly.

## [v1.9.0](../../../TrakHound/releases/tag/v1.9.3-beta)
- [x] Added Auto Updates
- [x] Cleaned up Plugin Interface Libraries
- [x] Cleaned up Server Plugins
- [x] Added Part Count Server Plugin

## [v1.8.0](../../../TrakHound/releases/tag/v1.8.0-beta)
- [x] Redesign Device Manager to be easier to manage multiple machines (10+)

## [v1.7.0](../../../TrakHound/releases/tag/v1.7.0-beta)
- [x] Add SQLite plugin to main project to enable use of SQLite databases

## [v1.6.2](../../../TrakHound/releases/tag/v1.6.2-beta)
- [x] Fixed [Issue #2](../../../TrakHound/issues/2)
- [x] Fixed [Issue #4](../../../TrakHound/issues/4)
- [x] Fixed [Issue #5](../../../TrakHound/issues/5)
- [x] Improved retrieving the list of Devices in TrakHound Client by only retrieving the list for Device Manager and then sending that list to all of the plugins. Plugins using the IClientPlugin interface are now required to use an ObservableCollection instead of a List and must handle any changes to the collection (if needed).
- [x] Added Table.Replace to TH_Database.IDatabasePlugin to remove the need to connect to a database twice when needing to Drop then Create a table. This was initially implemented to fix the issue of GenEventValues table not getting populated due to a connection error.
