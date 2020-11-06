module SimpBot
  module Animals
    module Errors
      class ApiTokenNotSetup < StandardError
        def initialize(variable)
          @variable = variable
        end

        def message
          """
            Cats api key couldn't be found in environmental variables.
            Please make sure that the #{@variable} variable is correctly set up.
          """
        end
      end
    end
  end
end
