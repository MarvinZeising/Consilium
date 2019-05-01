<template>
  <v-app>
    <Navbar v-if="isSignedIn" />
    <v-content>
      <router-view/>
    </v-content>
    <Footer v-if="isSignedIn" />
  </v-app>
</template>

<script lang="ts">
import Vue from 'vue'
import Navbar from '@/components/Navbar.vue'
import Footer from '@/components/Footer.vue'
import Component from 'vue-class-component'

@Component({
  components: {
    Navbar,
    Footer
  }
})
export default class App extends Vue {
  private isSignedIn: boolean = true
}
</script>
