require 'httparty'
require 'nokogiri'

module Astronomia
  class Api
    API_URL = "https://www.horoscope.com/star-ratings/today/"

    def initialize
      @repository = Repository.new
      @cache = HoroscopeCache.new
    end

    def horoscope_for_zodiac_sign zodiac_sign
      raise Errors::InvalidZodiacSignError unless Horoscope.is_valid_zodiac? zodiac_sign

      @cache.entry zodiac_sign do
        response = HTTParty.get(API_URL + zodiac_sign)
        parsed = Nokogiri::HTML(response.body)

        p Horoscope.new(
            zodiac_sign,
            'https://www.horoscope.com/' + parsed.xpath('/html/body/div[1]/div[3]/main/div/div/div[2]/img').first['src'],
            {
                stars: parsed.xpath('count(/html/body/div[1]/div[3]/main/div/h3[1]/i[contains(@class, "highlight")])').to_i,
                text: parsed.xpath("/html/body/div[1]/div[3]/main/div/p[1]/text()").to_s
            },
            {
                stars: parsed.xpath('count(/html/body/div[1]/div[3]/main/div/h3[2]/i[contains(@class, "highlight")])').to_i,
                text: parsed.xpath('/html/body/div[1]/div[3]/main/div/p[2]/text()').to_s
            },
            {
                stars: parsed.xpath('count(/html/body/div[1]/div[3]/main/div/h3[3]/i[contains(@class, "highlight")])').to_i,
                text: parsed.xpath('/html/body/div[1]/div[3]/main/div/p[3]/text()').to_s
            },
            {
                stars: parsed.xpath('count(/html/body/div[1]/div[3]/main/div/h3[4]/i[contains(@class, "highlight")])').to_i,
                text: parsed.xpath('/html/body/div[1]/div[3]/main/div/p[4]/text()').to_s
            },
            parsed.xpath('//*[@id="src-horo-matchlove"]/p/text()').to_s,
            parsed.xpath('//*[@id="src-horo-matchfriend"]/p/text()').to_s,
            parsed.xpath('//*[@id="src-horo-matchcareer"]/p/text()').to_s
        )
      end
    end

    def matches_for_zodiac_sign zodiac_sign
      raise Errors::InvalidZodiacSignError unless Horoscope.is_valid_zodiac? zodiac_sign

      @repository.find_users_by_zodiac_sign(zodiac_sign)
          .shuffle
          .take(5)
    end

    def horoscope_for_user user_id
      horoscope_for_zodiac_sign @repository.find_user_zodiac_sign user_id
    end

    def register_user user_id, zodiac_sign
      @repository.register_user user_id, zodiac_sign
      horoscope_for_zodiac_sign zodiac_sign
    end
  end
end