module SimpBot
  module Cats
    module Errors
      class ApiTokenNotSetup < StandardError
        def message
          ''"
            Cats api key couldn't be found in environmental variables.
            Please make sure that the CATS_API_KEY variable is correctly set up.
          "''
        end
      end
    end
  end
end