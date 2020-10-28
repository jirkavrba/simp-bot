require 'bundler/setup'
require 'require_all'

Bundler.setup :default

# Autoload the whole /src folder
require_all 'src'

# Validate that there is a token present in the environment variables
raise Astronomia::Errors::TokenNotSetupError if ENV["DISCORD_TOKEN"].nil?