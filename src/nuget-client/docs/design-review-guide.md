# Design guide and lifecycle

As an open source project, the NuGet.Client team does the large majority of its work in public GitHub repos.
All of the product code is public. The designs are also public, and for historical reasons split between the [Home](https://github.com/NuGet/Home/tree/dev/meta#nuget-proposal-process) repository and [Home wiki](https://github.com/NuGet/Home/wiki).

## Source controlled Design documentation

The design documentation lives in a source controlled repository that supports pull requests.

* The design reviews follow the same approach as code which allows us to use the vast tooling options available for pull request reviews.
* Pull requests have a clear lifecycle and allow everyone to contribute line by line comments, provide suggestions, hide resolved discussions and much more.

For for the dotnet OSS reader, our repository is the NuGet equivalent of the [dotnet/designs](https://github.com/dotnet/designs) repository.

## Asynchronous reviews

Not every design requires a meeting. Not everything needs to or can be resolved in a single 1 hour meeting.
Treating designs like code encourages higher participation, as the reader does not need to be online and available at the same time as others to review. Reviewers can also spend longer time internalizing a design than can feasibly be done in one meeting.

Certain designs will require meetings, and you are encouraged to schedule as many as needed. These are just guidelines. Try to be as inclusive and as appreciative of people's time and effort as possible.

## Participation

It is the designer's responsibility to include all relevant stakeholders in their design meetings.

* If you are an engineer on the NuGet.Client team you are responsible for identifying stakeholders. For help identifying the relevant audience, engage with the leads, PMs and more senior members of the team.
* If you a community contributor, either 1st part or 3rd party, your design will be assigned a shepherd to help drive the discussion.

The core participation rules are as follows:

* All NuGet engineers are to included in the final phase for all designs. All engineers should be requested as reviewers and invited to the review meetings.
  * Not everyone needs to, or is required to participate.
* For designs that contain an experience change, a Program Manager needs to be involved.
* For any changes that affect the feeds, the NuGet Server team needs to be involved.
* When necessary, involve partner teams & customers before finally confirming a design.
* Finally, involve the community and contributors, by chiming in on the tracking issue for the relevant design.
* When scheduling meetings, do provide an agenda, so that people uninterested and unaffected can skip the meeting as needed.

## Design rules

1. Start by using the [template.md](https://github.com/NuGet/Home/blob/dev/meta/template.md).
1. All public facing designs live in the [Home](https://github.com/NuGet/Home/tree/dev/meta#nuget-proposal-process) repo.
1. Designs that do not affect the product, guides, etc, live in the private [Client.Engineering](https://github.com/NuGet/Client.Engineering/tree/main/designs)) repo. Sometimes early iterations of a design stay private, and the Client.Engineering repository can be used for that.
1. Follow the usual PR lifecycle to get the design to the best state that it can be and merge when accepted.
See the [NuGet Proposal Process](https://github.com/NuGet/Home/tree/dev/meta#nuget-proposal-process).

The above workflow was borrowed from [MS CSE playbook](https://github.com/microsoft/code-with-engineering-playbook/blob/main/docs/design/design-reviews/recipes/async-design-reviews.md).
