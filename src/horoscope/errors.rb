# frozen_string_literal: true

module SimpBot
  module Horoscope
    module Errors
      class UserZodiacMissingError < StandardError
        def message
          "Given user doesn't have a zodiac sign registered.\nPlease register it with `+horoscope register [sign]`"
        end
      end

      class InvalidArgumentError < StandardError
        def message
          "The command was called with an invalid number of arguments.\nCorrect syntax is:\n - for users with saved zodiac sign: `+horoscope`\n- for registering: `+horoscope register [sign]`\ - for a specified zodiac sign: `+horoscope [sign]`\n"
        end
      end

      class InvalidZodiacSignError < StandardError
        def message
          "The given zodiac sign is invalid.\nAvailable signs are: `#{SimpBot::Horoscope::ZODIAC_SIGNS.join '` `'}`"
        end
      end
    end
  end
end
