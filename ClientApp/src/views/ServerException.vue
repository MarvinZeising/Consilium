<template>
  <v-layout
    align-center
    justify-center
    column
    fill-height
  >
    <h1 class="mb-4">Cannot connect to server</h1>
    <p>Probably due to maintenance, the server is not reachable at the moment.</p>
    <p>Please try again later</p>
    <v-btn
      class="mt-3"
      color="warning"
      @click.stop="tryAgainNow"
    >
      Try again now
    </v-btn>
  </v-layout>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'

@Component
export default class ServerException extends Vue {
  private tryAgainNow() {
    location.assign('/')
  }
}
</script>
