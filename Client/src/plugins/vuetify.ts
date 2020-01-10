import Vue from 'vue'
import Vuetify from 'vuetify/lib'
import colors from 'vuetify/lib/util/colors'
import 'vuetify/dist/vuetify.min.css'

Vue.use(Vuetify)

export default new Vuetify({
  theme: {
    themes: {
      light: {
        navbar: colors.blue.darken3,
        primary: colors.blue.darken3,
        secondary: colors.amber.base,
        accent: colors.pink.base,
        error: colors.red.base,
        warning: colors.orange.base,
        info: colors.cyan.base,
        success: colors.green.base,
      },
      dark: {
        navbar: colors.blue.darken3,
        primary: colors.blue.base,
        secondary: colors.amber.base,
        accent: colors.pink.base,
        error: colors.red.base,
        warning: colors.orange.base,
        info: colors.cyan.base,
        success: colors.green.base,
      },
    },
    dark: false,
  },
  iconfont: 'md',
});
