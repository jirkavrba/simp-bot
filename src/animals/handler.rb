require_relative '../handler'

module SimpBot
  module Animals
    class MessageHandler < MessageHandlerBase

      ENDPOINTS = [
          {
              commands: ["+cat", "+pussy"],
              url: "https://api.thecatapi.com/v1/images/search",
              headers: {
                  "x-api-key" => ENV.fetch("CATS_API_KEY") {
                    raise Animals::Errors::ApiTokenNotSetup.new "CATS_API_KEY"
                  }
              },
              title: "Meow!",
              extract: -> (json) do json[0]["url"] end
          }
      ]

      def initialize(repository)
        super repository
      end

      def handle_message(event)
        endpoint = ENDPOINTS.find do |endpoint| endpoint[:commands].include? event.message.content end

        unless endpoint.nil?
          response = HTTParty.get(endpoint[:url], headers: endpoint[:headers] || []).parsed_response

          embed = Discordrb::Webhooks::Embed.new

          embed.title = endpoint[:title]
          embed.image = Discordrb::Webhooks::EmbedImage.new url: endpoint[:extract].call(response)
          embed.timestamp = event.timestamp
          embed.footer = Discordrb::Webhooks::EmbedFooter.new text: event.message.user.username,
                                                              icon_url: event.message.user.avatar_url


          event.channel.send_message(nil, nil, embed)
        end
      end
    end
  end
end