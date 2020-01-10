<template>
  <v-container fluid>
    <v-layout wrap>

      <ParticipantsInvitations />

      <ParticipantsRequests />

      <ParticipantsList />

    </v-layout>
  </v-container>
</template>

<script lang="ts">
import { Vue, Component, Watch } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import { Project } from '../../models'
import ParticipantsInvitations from '../../components/ParticipantsInvitations.vue'
import ParticipantsRequests from '../../components/ParticipantsRequests.vue'
import ParticipantsList from '../../components/ParticipantsList.vue'

@Component({
  components: {
    ParticipantsInvitations,
    ParticipantsRequests,
    ParticipantsList,
  }
})
export default class Participants extends Vue {
}
</script>
