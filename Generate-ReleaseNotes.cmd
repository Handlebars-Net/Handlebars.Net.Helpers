rem https://github.com/StefH/GitHubReleaseNotes

SET version=2.3.7

GitHubReleaseNotes --output CHANGELOG.md --skip-empty-releases --exclude-labels question invalid documentation duplicate --version %version% --token %GH_TOKEN%