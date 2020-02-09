import Vue from 'vue'
import VueI18n, { LocaleMessages } from 'vue-i18n'

Vue.use(VueI18n)

function loadLocaleMessages(): LocaleMessages {
  const locales = require.context('./locales', true, /[A-Za-z0-9-_,\s]+\.json$/i)
  const messages: LocaleMessages = {}

  locales.keys().forEach((key) => {
    const localeMatched = key.match(/\/([A-Za-z-]+)\//i)
    const areaMatched = key.match(/([A-Za-z]+)\./i)

    if (localeMatched && areaMatched) {
      const locale = localeMatched[1]
      const area = areaMatched[1]

      if (!messages[locale]) {
        messages[locale] = {}
      }

      messages[locale][area] = locales(key)
    }
  })
  return messages
}

export default new VueI18n({
  locale: 'en-US',
  fallbackLocale: 'en-US',
  messages: loadLocaleMessages()
})
