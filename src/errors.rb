module SimpBot
  module Errors
    class TokenNotSetupError < StandardError
      def message
        "Discord bot token couldn't be found in environmental variables.\nPlease make sure that the DISCORD_TOKEN variable is correctly set up."
      end
    end
  end
end
