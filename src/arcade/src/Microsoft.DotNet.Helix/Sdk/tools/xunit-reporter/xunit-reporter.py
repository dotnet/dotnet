#!/usr/bin/env python

import helix.event
import helix.settings

if __name__ == '__main__':
    settings = helix.settings.settings_from_env()
    event_client = helix.event.create_from_uri(settings.event_uri)
    event_client.warning(settings, "Obsolete", "xunit-reporter.py is deprecated, please remove 'EnableXUnitReporter' from your build and use 'EnableAzurePipelinesReporter' instead.")
    print("This reporter is deprecated")
