module Astronomia
  module Errors

    class TokenNotSetupError < StandardError
      def full_message
        """
          Discord bot token couldn't be found in environmental variables.
          Please make sure that the DISCORD_TOKEN variable is correctly set up.
        """
      end
    end

  end
end
