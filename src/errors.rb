# frozen_string_literal: true

module Astronomia
  module Errors
    class TokenNotSetupError < StandardError
      def message
        ''"
          Discord bot token couldn't be found in environmental variables.
          Please make sure that the DISCORD_TOKEN variable is correctly set up.
        "''
      end
    end

    class UserZodiacMissingError < StandardError
      def message
        ''"
          Given user doesn't have a zodiac sign registered.
          Please register it with `+horoscope register [sign]`
        "''
      end
    end

    class InvalidArgumentError < StandardError
      def message
        ''"
          The command was called with an invalid number of arguments.

          Correct syntax is:
          - for users with saved zodiac sign: `+horoscope`
          - for registering: `+horoscope register [sign]`
          - for a specified zodiac sign: `+horoscope [sign]`
        "''
      end
    end

    class InvalidZodiacSignError < StandardError
      def message
        ''"
          The given zodiac sign is invalid.
          Available signs are: `#{Astronomia::ZODIAC_SIGNS.join '` `'}`
        "''
      end
    end
  end
end
