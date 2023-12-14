rem https://github.com/StefH/GitHubReleaseNotes

SET version=2.4.1.3

GitHubReleaseNotes --output CHANGELOG.md --skip-empty-releases --exclude-labels question invalid documentation duplicate --version %version% --token %GH_TOKEN%