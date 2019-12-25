<template>
  <v-flex xs12 sm10 md8 lg6 xl4>
    <h2
      class="headline mb-3"
      v-t="'person.persons'"
    />
    <v-card flat class="ma-2 mb-5">
      <v-card-text
        class="grey--text"
        v-t="'project.personsDescription'"
      />
      <v-list>
        <v-list-item
          v-if="getPersons.length === 0"
          dark
          class="warning"
        >
          <span v-t="'project.noPersons'" />
        </v-list-item>
        <v-list-item
          v-for="(person, index) in getPersons"
          :key="index"
        >
          <v-list-item-content>
            <v-list-item-title v-text="person.fullName()" />
          </v-list-item-content>

          <v-list-item-action>
            <v-btn
              icon
              class="ma-0"
              @click=""
            >
              <v-icon color="grey">settings</v-icon>
            </v-btn>
          </v-list-item-action>
        </v-list-item>
      </v-list>
      <v-card-actions>
        <v-spacer />
        <v-btn
          text
          v-t="'project.invitePerson'"
        />
      </v-card-actions>
    </v-card>
  </v-flex>
</template>

<script lang="ts">
import { Vue, Component, Watch } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import PersonModule from '../store/persons'
import ProjectModule from '../store/projects'
import { Person } from '../models/definitions'

@Component
export default class PersonsPending extends Vue {
  private personModule: PersonModule = getModule(PersonModule, this.$store)
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)

  private getPersons() {
    const projectId = this.$route.params.projectId
    const participation = this.getProjectParticipation(projectId)
    return participation?.status || 'none'
  }

  private getProjectParticipation(projectId: string) {
    return this.projectModule.getParticipations.find((p) => p.projectId === projectId)
  }

}
</script>
