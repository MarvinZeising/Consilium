<template>
  <v-app>
    <Navbar />
    <v-content>
      <router-view/>
    </v-content>
  </v-app>
</template>

<script lang="ts">
import Vue from 'vue';
import Navbar from '@/components/Navbar.vue';

export default Vue.extend({
  name: 'ConsiliumApp',
  components: { Navbar },
  data() {
    return {
    };
  }
});
</script>
