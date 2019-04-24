<template>
  <div class="joinProject">
    <!--//* Join Project -->
    <v-flex xs12>
      <h2 class="headline mb-3">Join an existing Project</h2>
    </v-flex>
    <v-flex
      xs12 sm8 md6 lg4
      class="mb-5"
    >
      <p>If you received a Project access token, you can enter the token here to join that Project:</p>
      <v-text-field
        v-model="accessToken"
        label="Project access token"
      ></v-text-field>
      <v-btn
        :disabled="accessToken == ''"
        color="primary"
        >
        Join this Project
      </v-btn>
    </v-flex>
  </div>
</template>

<script lang="ts">
import Vue from 'vue'
export default Vue.extend({
  data() {
    return {
      accessToken: null
    }
  }
})
</script>
