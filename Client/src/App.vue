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

export default Vue.extend({
  name: 'ConsiliumApp',
  components: { Navbar, Footer },
  data() {
    return {
      isSignedIn: true
    }
  }
})
</script>
