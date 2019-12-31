<template>
  <v-flex xs12 sm10 md8 lg6 xl4>
    <h2
      class="headline mb-3"
      v-t="'project.requests'"
    />
    <v-card flat class="ma-2 mb-5">
      <v-card-text
        class="grey--text"
        v-t="'project.requestsDescription'"
      />
      <v-list>
        <v-list-item
          v-if="getRequests.length === 0"
          dark
          class="warning"
        >
          <span v-t="'project.noRequests'" />
        </v-list-item>
        <v-list-item
          v-for="(participation, index) in getRequests"
          :key="index"
          two-line
        >
          <v-list-item-content>
            <v-list-item-title v-text="getPerson(participation.personId).fullName()" />
            <v-list-item-subtitle>Requested on Dec 12, 2019</v-list-item-subtitle>
          </v-list-item-content>

          <v-list-item-action>
            <v-btn
              text
              class="mt-2"
              @click=""
              v-t="'core.handle'"
            />
          </v-list-item-action>
        </v-list-item>
      </v-list>
      <v-card-actions>
        <v-switch
          v-model="requestsAllowed"
          :label="$t(`project.requests${requestsAllowed ? '' : 'Not'}Allowed`)"
          class="mt-0 mb-3 ml-3"
          color="primary"
          inset
          hide-details
        />
      </v-card-actions>
    </v-card>
  </v-flex>
</template>

<script lang="ts">
import { Vue, Component, Watch } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import i18n from '../i18n'
import ProjectModule from '../store/projects'
import { Person, ProjectParticipationStatus, Gender } from '../models/definitions'

@Component
export default class PersonsRequests extends Vue {
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)

  private requestsAllowed: any = true

  private get getRequests() {
    const projectId = this.$route.params.projectId

    return this.projectModule.getParticipations
      .filter((p) => p.projectId === projectId && p.status === ProjectParticipationStatus.Requested)
  }

  private  getPerson(personId: string) {
    return this.projectModule.getProjectPersons.find((x: Person) => x.id === personId)
  }

}
</script>
