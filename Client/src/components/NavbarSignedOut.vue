<template>
  <v-navigation-drawer
    app
    v-model='drawer'
    :clipped="$vuetify.breakpoint.lgAndUp"
  >

    <v-list nav>

      <!--//* Sign in -->
      <v-list-item :to="{ name: 'signIn' }">
        <v-list-item-content>
          <v-list-item-title>{{ $t('navbar.signIn') }}</v-list-item-title>
        </v-list-item-content>
      </v-list-item>

      <!--//* Sign up -->
      <v-list-item :to="{ name: 'signUp' }">
        <v-list-item-content>
          <v-list-item-title>{{ $t('navbar.signUp') }}</v-list-item-title>
        </v-list-item-content>
      </v-list-item>

    </v-list>

  </v-navigation-drawer>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'

@Component
export default class NavbarSignedOut extends Vue {

  @Prop(Boolean)
  private readonly drawer?: boolean

}
</script>
