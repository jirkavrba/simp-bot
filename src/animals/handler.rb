require_relative "../handler"

module SimpBot
  module Animals
    class MessageHandler < MessageHandlerBase
      ENDPOINTS = [
        {
          commands: %w[+cat +catto +pussy],
          url: "https://api.thecatapi.com/v1/images/search",
          headers: {
            "x-api-key" => ENV.fetch("CATS_API_KEY") {
              raise Animals::Errors::ApiTokenNotSetup.new "CATS_API_KEY"
            }
          },
          title: "Meow!",
          extract: -> json { json[0]["url"] }
        },
        {
          commands: %w[+dog +doggo],
          url: "https://random.dog/woof.json",
          headers: {},
          title: "Woof!",
          extract: -> json { json["url"] }
        },
        {
          commands: %w[+fox +foxxo],
          url: "https://randomfox.ca/floof/",
          headers: {},
          title: "What does the fox say?",
          extract: -> json { json["image"] }
        },
        {
          commands: %w[+duck +quack],
          url: "https://random-d.uk/api/v1/random?type=png",
          headers: {},
          title: "Quack!",
          extract: -> json { json["url"] }
        },
        {
          commands: ["+panda"],
          url: "https://some-random-api.ml/img/panda",
          headers: {},
          title: "Here, have a panda!",
          extract: -> json { json["link"] }
        },
        {
          commands: %w[+bird +birb],
          url: "https://some-random-api.ml/img/birb",
          headers: {},
          title: "Tweet!",
          extract: -> json { json["link"] }
        },
        {
          commands: ["+koala"],
          url: "https://some-random-api.ml/img/koala",
          headers: {},
          title: "A wild koala appears!",
          extract: -> json { json["link"] }
        },
        {
            commands: ["+sloth"],
            url: "https://sloth.pics/api",
            headers: {},
            title: "Man, this API is kinda sloooow",
            extract: -> json { json["url"] }
        }
      ]

      def initialize(repository)
        super repository
      end

      def handle_message(event)
        endpoint = ENDPOINTS.find { |endpoint|
          endpoint[:commands].include? event.message.content
        }

        unless endpoint.nil?
          response = HTTParty.get(endpoint[:url], headers: endpoint[:headers] || []).parsed_response
          image = endpoint[:extract].call(response)

          embed = Discordrb::Webhooks::Embed.new

          embed.title = endpoint[:title]
          embed.timestamp = event.timestamp
          embed.footer = Discordrb::Webhooks::EmbedFooter.new text: event.message.user.username,
                                                              icon_url: event.message.user.avatar_url

          if %w[.mp4 .webm].any? do |format|
            image.end_with? format
          end
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
