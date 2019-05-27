<template>
  <v-container fluid fill-height>
    <v-layout align-center justify-center>
      <h3
        class="headline"
        v-t="'account.signingOut'"
      />
    </v-layout>
  </v-container>
</template>

<script lang="ts">
import Vue from 'vue'
import UserModule from '@/store/modules/users'
import { getModule } from 'vuex-module-decorators'
import Component from 'vue-class-component'

@Component
export default class SignOut extends Vue {
  private userModule: UserModule = getModule(UserModule, this.$store)

  private async created() {
    await this.userModule.signOut()
    this.$router.replace({ name: 'signIn' })
  }
}
</script>
