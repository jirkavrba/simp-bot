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
          },
          {
              commands: ["+dog", "+doggo"],
              url: "https://random.dog/woof.json",
              headers: {},
              title: "Woof!",
              extract: -> (json) do p json; json["url"] end
          }
      ]

      def initialize(repository)
        super repository
      end

      def handle_message(event)
        endpoint = ENDPOINTS.find do |endpoint| endpoint[:commands].include? event.message.content end

        unless endpoint.nil?
          response = HTTParty.get(endpoint[:url], headers: endpoint[:headers] || []).parsed_response
          image = endpoint[:extract].call(response)

          embed = Discordrb::Webhooks::Embed.new

          embed.title = endpoint[:title]
          embed.timestamp = event.timestamp
          embed.footer = Discordrb::Webhooks::EmbedFooter.new text: event.message.user.username,
                                                              icon_url: event.message.user.avatar_url

          if [".mp4", ".webm"].any? do |format| image.end_with? format end
            embed.description = image
          else
            embed.image = Discordrb::Webhooks::EmbedImage.new url: image
          end

          event.channel.send_message(nil, nil, embed)
        end
      end
    end
  end
end