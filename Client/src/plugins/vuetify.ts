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
        accent: colors.amber.lighten2,
        error: colors.red.base,
        warning: colors.orange.base,
        info: colors.cyan.base,
        success: colors.green.base,
      },
      dark: {
        navbar: colors.blue.darken3,
        primary: colors.blue.base,
        accent: colors.amber.darken4,
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
