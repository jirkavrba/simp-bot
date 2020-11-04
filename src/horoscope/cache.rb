# frozen_string_literal: true

module SimpBot
  module Horoscope
    CacheEntry = Struct.new(:value, :inserted_at) { ; }

    class HoroscopeCache
      def initialize
        @storage = {}
      end

      def entry(zodiac_sign, &block)
        if @storage.key?(zodiac_sign) && is_still_valid?(@storage[zodiac_sign])
          @storage[zodiac_sign].value
        else
          value = block.call
          @storage[zodiac_sign] = CacheEntry.new value, today_date

          value
        end
      end

      private

      def is_still_valid?(entry)
        entry.inserted_at == today_date
      end

      def today_date
        time = Time.now.utc + Time.zone_offset('PDT')
        time.to_date
      end
    end
  end
end
