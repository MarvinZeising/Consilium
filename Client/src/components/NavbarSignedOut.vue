<template>
  <div>

    <v-list
      nav
      dense
    >

      <!--//* Sign in -->
      <v-list-item :to="{ name: 'signIn' }">
        <v-list-item-icon>
          <v-icon>fingerprint</v-icon>
        </v-list-item-icon>
        <v-list-item-content>
          <v-list-item-title>{{ $t('navbar.signIn') }}</v-list-item-title>
        </v-list-item-content>
      </v-list-item>

      <!--//* Sign up -->
      <v-list-item :to="{ name: 'signUp' }">
        <v-list-item-icon>
          <v-icon>person_add</v-icon>
        </v-list-item-icon>
        <v-list-item-content>
          <v-list-item-title>{{ $t('navbar.signUp') }}</v-list-item-title>
        </v-list-item-content>
      </v-list-item>

    </v-list>

  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'

@Component
export default class NavbarSignedOut extends Vue {

  @Prop(Boolean)
  private readonly drawer?: boolean

}
</script>
