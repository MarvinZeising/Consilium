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
        :items="participants"
        :items-per-page="15"
        :loading="loading"
        loading-text="Loading..."
      />
    </v-card>
  </v-flex>
</template>

<script lang="ts">
import { Vue, Component, Watch } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import moment from 'moment'
import UserModule from '../store/users'
import ParticipantModule from '../store/participants'
import { Person, ParticipationStatus, Gender } from '../models/definitions'

@Component
export default class ParticipantsList extends Vue {
  private userModule: UserModule = getModule(UserModule, this.$store)
  private participantModule: ParticipantModule = getModule(ParticipantModule, this.$store)

  private loading: boolean = true

  private participants: any[] = []
  private headers: any[] = [
    { text: 'First name', value: 'firstname' },
    { text: 'Last name', value: 'lastname' },
    { text: 'Role', value: 'role' },
  ]

  @Watch('$route')
  private async onRouteChanged(val: string, oldVal: string) {
    await this.init()
  }

  private async created() {
    this.init()
  }

  private async init() {
    await this.participantModule.loadParticipants()

    this.participants = this.participantModule.getParticipations.map((participation) => {
      return {
        firstname: participation.person?.firstname,
        lastname: participation.person?.lastname,
        role: participation.role?.name,
      }
    })

    this.loading = false
  }

}
</script>
