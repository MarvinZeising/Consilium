<template>
  <v-app>
    <Navbar v-if="isSignedIn" />
    <v-content>
      <router-view/>
    </v-content>
    <Footer v-if="isSignedIn" />

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
        @click="alertModule.getSnackbar.show = false"
      >
        Close
      </v-btn>
    </v-snackbar>
  </v-app>
</template>

<script lang="ts">
import Vue from 'vue'
import Component from 'vue-class-component'
import { getModule } from 'vuex-module-decorators'
import Navbar from './components/Navbar.vue'
import Footer from './components/Footer.vue'
import AlertModule from './store/modules/alerts'

@Component({
  components: {
    Navbar,
    Footer
  }
})
export default class App extends Vue {
  private alertModule: AlertModule = getModule(AlertModule, this.$store)
  private isSignedIn: boolean = true
}
</script>
