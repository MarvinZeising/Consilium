<template>
  <v-flex xs12>
    <h2 class="headline mb-3">
      {{ $tc('project.participant.participants', 2) }}
    </h2>
    <v-card
      flat
      class="ma-2 mb-5"
    >
      <v-data-table
        :headers="headers"
        :items="items"
        :items-per-page="15"
      />
    </v-card>
  </v-flex>
</template>

<script lang="ts">
import { Vue, Component, Watch } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import moment from 'moment'
import UserModule from '../store/users'
import { Person, ParticipationStatus, Gender } from '../models/definitions'

@Component
export default class ParticipantsList extends Vue {
  private userModule: UserModule = getModule(UserModule, this.$store)

  private headers: any[] = [
    { text: 'First name', value: 'firstname' },
    { text: 'Last name', value: 'lastname' },
    { text: 'Role', value: 'role' },
  ]

  private items: any[] = [
    {
      firstname: 'Marvin',
      lastname: 'Zeising',
      role: 'Administrator',
    },
  ]

}
</script>
