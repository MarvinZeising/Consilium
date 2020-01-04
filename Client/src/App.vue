<template>
  <v-app>
    <Navbar />
    <v-content>
      <router-view/>
    </v-content>

    <v-snackbar
      v-model="alertModule.getSnackbar.show"
      :color="alertModule.getSnackbar.color"
      :top="alertModule.getSnackbar.y === 'top'"
      :bottom="alertModule.getSnackbar.y === 'bottom'"
      :left="alertModule.getSnackbar.x === 'left'"
      :right="alertModule.getSnackbar.x === 'right'"
      :vertical="alertModule.getSnackbar.mode === 'vertical'"
      :multi-line="alertModule.getSnackbar.mode === 'multi-line'"
      :timeout="alertModule.getSnackbar.timeout"
    >
      {{ alertModule.getSnackbar.text }}
      <v-btn
        dark
        text
        @click.stop="alertModule.getSnackbar.show = false"
      >
        Close
      </v-btn>
    </v-snackbar>
  </v-app>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import Navbar from './components/Navbar.vue'
import AlertModule from './store/alerts'
import UserModule from './store/users'

@Component({
  components: { Navbar }
})
export default class App extends Vue {
  private alertModule: AlertModule = getModule(AlertModule, this.$store)
}
</script>
