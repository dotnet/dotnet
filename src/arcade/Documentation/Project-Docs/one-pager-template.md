# Epic Template - One-Pagers Guidelines

## Goal and Motivation

The information included within our epics are high level business objectives and does not always leave much room for practical information. Sometime the v-team is able to capture all the information listed within outlined below in the epic. If that is the case, there is no need for an additional document.

In most cases, however, v-teams need a place where they can capture additional information that helps them "think about" how they are going to implement a given feature.

The goal of the one-pager is to bring clarity to how the v-team is going to implement and support specific aspects of the business goals defined in the epic.

The document below is meant to be a guideline on what the v-team should be thinking about when defining the feature they are working on. It is up to you what you include in your one pager.

## One-Pager Guidelines

In this section you will find the areas that you should consider including in your one-pager.

### Stakeholders

Who is this work for (i.e. stakeholder and those that should "sign-off" on your POC) and what are the problem(s) they asking us to solve?  

### Proof of Concept (POC)

An effective proof of concept proves the goal of a proposed project is viable, and will be successful. The value of a POC is it can help the v-team identify gaps in processes that might interfere with success.

A POC can help
- Elicits feedback from everyone involved in a project, including those who might not have otherwise contributed, thereby mitigating unforeseen risk.
- Creates a test project to evaluate before work begins on an actual project. 
- Verifies that concepts and theories applied to a project will have a real-world application.
- Helps us to prove our assumptions (for example, if certain functionality, like using a service account to post comments from GitHub to Teams in a service, is possible) before committing to completing the work in a given timeframe.
- Helps us to adjust our expectations about how much work a feature might take to complete depending on the challenges we run into that we didn't originally consider in our assumptions.
- Can have more than one POC, if necessary, for a project.


### Risk

- Will the new implementation of any existing functionality cause breaking changes for existing consumers?
- What are your assumptions?
- What are your unknowns?
- What dependencies will this epic/feature(s) have?
  - Are the dependencies currently in a state that the functionality in the epic can consume them now, or will they need to be updated?
- Is there a goal to have this work completed by, and what is the risk of not hitting that date? (e.g. missed OKRs, increased pain-points for consumers, functionality is required for the next product release, et cetera)
- Does anything the new feature depend on consume a limited/throttled API resource? 
- Have you estimated what maximum usage is? 
- Are you utilizing any response data that allows intelligent back-off from the service?
- What is the plan for getting more capacity if the feature both must exist and needs more capacity than available?

### Usage Telemetry

- How are we measuring the “usefulness” to the stakeholders of the business objectives?
- How are we tracking the usage of this new feature?

## Service-ability of Feature

Changes that we implement often require addition maintenance to support them long term. The FR group has been set up to handle this work but it is up to the v-team to make sure FR is successful in servicing the changes made within your epic long term. Please see the [Servicing Guidelines](https://github.com/dotnet/arcade/blob/main/Documentation/Project-Docs/Servicing%20Guidelines.md) Document for what you should be thinking about during your feature creation to help the team be able to easily service your feature long term.

## House Keeping

In order to align with Epic Content Guidance, one-pagers should be stored in a central location. 
- The folder to store your One-Pager can be found in the [Documentation Folder](https://github.com/dotnet/arcade/tree/main/Documentation/TeamProcess/One-Pagers)
- The name the one-pager should contain the name of the epic and the epic issue number (for easy reference). 
  - Example: Coordinate migration from "master" to "main" in all dotnet org repos - core-eng10412.md. 
- Use the PR process to document the discussion around the content of the one-pager.

Guidance for Epics can be found at [Guidelines for Epics](https://dev.azure.com/dnceng/internal/_wiki/wikis/DNCEng%20Services%20Wiki/552/Guidelines-for-Epics) wiki.

After all discussions have been resolved, the resulting one-pager document should be signed-off (this does not need to be a formal process) by stakeholders (e.g. v-team members, epic owners, et cetera) and then linked to the associated epic's GitHub issue for discover-ability.


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CProject-Docs%5Cone-pager-template.md)](https://helix.dot.net/f/p/5?p=Documentation%5CProject-Docs%5Cone-pager-template.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CProject-Docs%5Cone-pager-template.md)</sub>
<!-- End Generated Content-->
