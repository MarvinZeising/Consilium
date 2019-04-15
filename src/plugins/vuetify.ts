import Vue from 'vue';
import Vuetify from 'vuetify';
import 'vuetify/dist/vuetify.min.css';
import colors from 'vuetify/es5/util/colors'


Vue.use(Vuetify, {
  theme: {
    primary: colors.indigo.base,
    secondary: colors.pink.base,
    accent: colors.amber.base,
    error: colors.red.base,
    warning: colors.orange.base,
    info: colors.cyan.base,
    success: colors.green.base
  },
  iconfont: 'md',
});
