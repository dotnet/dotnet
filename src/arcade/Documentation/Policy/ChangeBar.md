# Bar for when changes are taken
The criteria for *when* changes can be taken into Arcade are as follows.  See farther down the document for the bar (criteria) for determining the *type* of change.

## Breaking Changes
| Area              | When                   | Notes / Exceptions          |
| -------------     | ---------------------- | --------------------------- |
| Shared Tools      | Major version change   | Extraordinary business need | 
| Services          | Major version change   | Extraordinary business need | 
| Backing Resources | Major version change   | Extraordinary business need | 
| Guidance          | Major version change   | Extraordinary business need | 

## Disruptive Changes
| Area              | When                              | Notes / Exceptions          |
| -------------     | ----------------------            | --------------------------- |
| Shared Tools      | When product is *not* stabilizing | Should be < 1 a month       | 
| Services          | When product is *not* stabilizing | Should be < 1 a month       |
| Backing Resources | When product is *not* stabilizing | | 
| Guidance          | Whenever needed                   | Adoption will take longer   | 

## Minimal risk Changes
| Area              | When                              | Notes / Exceptions          |
| -------------     | ----------------------            | --------------------------- |
| Shared Tools      | Ok in master only                 | | 
| Services          | When product is *not* stabilizing | |
| Backing Resources | When product is *not* stabilizing | | 
| Guidance          | Whenever needed                   | | 

## Minor, low risk Changes
| Area              | When                              | Notes / Exceptions          |
| -------------     | ----------------------            | --------------------------- |
| Shared Tools      | Ok in master only                 | | 
| Services          | When product is *not* stabilizing | |
| Backing Resources | When product is *not* stabilizing | | 
| Guidance          | Whenever needed                   | | 

# Bar for determining class of change

| Change Type     | Criteria |
| -------------   | ---------------| 
| Breaking        | Change breaks a scenario, requiring an update by our customer/s |
| Disruptive      | Not necessarily breaking, but there's a high likelihood there will be some unintended fallout **OR** is a change which adds debt|
| Minimal Risk    | Probably won't break anyone or cause disruption |
| Minor, low risk | Very unlikely to have any negative impact |




<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CPolicy%5CChangeBar.md)](https://helix.dot.net/f/p/5?p=Documentation%5CPolicy%5CChangeBar.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CPolicy%5CChangeBar.md)</sub>
<!-- End Generated Content-->
