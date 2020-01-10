<template>
  <div>

    <!-- //* Person -->
    <v-menu
      v-if="personModule.getActivePerson && personModule.getPersons.length > 1"
      transition="slide-y-transition"
      bottom
    >
      <template v-slot:activator="{ on: person }">
        <v-list>
          <v-list-item
            link
            two-line
            v-on="{ ...person }"
          >
            <v-list-item-content>
              <v-list-item-title class="title">
                {{ personModule.getActivePerson.getFullName }}
              </v-list-item-title>
              <v-list-item-subtitle>
                {{ personModule.getActivePerson.getCongregationName }}
              </v-list-item-subtitle>
            </v-list-item-content>
            <v-list-item-icon>
              <v-icon right>arrow_drop_down</v-icon>
            </v-list-item-icon>
          </v-list-item>
        </v-list>
      </template>
      <v-list>
        <v-list-item
          v-for="(person, i) in personModule.getPersons"
          :key="i"
          @click="activatePersonIfNotMe(person.id)"
        >
          <v-list-item-title>
            <v-icon left>person</v-icon>
            <span>{{ person.getFullName }}</span>
          </v-list-item-title>
        </v-list-item>
      </v-list>
    </v-menu>
    <v-list v-else-if="personModule.getActivePerson">
      <v-list-item two-line>
        <v-list-item-content>
          <v-list-item-title class="title">
            {{ personModule.getActivePerson.getFullName }}
          </v-list-item-title>
          <v-list-item-subtitle>
            {{ personModule.getActivePerson.getCongregationName }}
          </v-list-item-subtitle>
        </v-list-item-content>
      </v-list-item>
    </v-list>

    <v-divider></v-divider>

    <v-list
      nav
      dense
    >

      <!--//* Home -->
      <v-list-item
        link
        color="primary"
        :to="{ name: 'home' }"
      >
        <v-list-item-icon>
          <v-icon>home</v-icon>
        </v-list-item-icon>
        <v-list-item-content>
          <v-list-item-title v-t="'core.home'"/>
        </v-list-item-content>
      </v-list-item>

      <!--//* Projects -->
      <div v-if="personModule.getActivePerson">
        <v-list-group
          v-for="(participation, i) in personModule.getActivePerson.getParticipations"
          :key="i"
          prepend-icon="extension"
        >
          <template v-slot:activator>
            <v-list-item-content>
              <v-list-item-title v-text="participation.project.name" />
              <v-list-item-subtitle
                v-if="participation.status == 'invited'"
                v-t="'project.invitation.info'"
              />
              <v-list-item-subtitle
                v-if="participation.status == 'requested'"
                v-t="'project.request.info'"
              />
            </v-list-item-content>
          </template>

          <v-list-item
            v-if="isPendingParticipation(participation)"
            :to="{ name: 'configureProjects' }"
          >
            <v-list-item-icon />
            <v-list-item-content>
              <v-list-item-title v-t="'project.checkStatus'" />
            </v-list-item-content>
          </v-list-item>
          <div v-else>

            <v-list-item
              v-if="canView(participation, 'calendar')"
              :to="{ name: 'calendar', params: { projectId: participation.project.id }}"
            >
              <v-list-item-icon />
              <v-list-item-content>
                <v-list-item-title v-t="'project.calendar'" />
              </v-list-item-content>
            </v-list-item>

            <v-list-group
              v-if="canView(participation, 'topics') && participation.project.getTopics.length > 0"
              no-action
              sub-group
            >
              <template v-slot:activator>
                <v-list-item-content>
                  <v-list-item-title v-t="'project.knowledgeBase.knowledgeBase'" />
                </v-list-item-content>
              </template>

              <v-list-item
                v-for="(topic, j) in participation.project.getTopics"
                :key="j"
                :to="{ name: 'topic', params: { projectId: participation.project.id, topicId: topic.id }}"
              >
                <v-list-item-content>
                  <v-list-item-title v-text="topic.name" />
                </v-list-item-content>
              </v-list-item>
            </v-list-group>

            <v-list-group
              v-if="getAdminActions(participation).length > 0"
              no-action
              sub-group
            >
              <template v-slot:activator>
                <v-list-item-content>
                  <v-list-item-title v-t="'project.administration'" />
                </v-list-item-content>
              </template>

              <v-list-item
                v-for="(action, j) in getAdminActions(participation)"
                :key="j"
                :to="{ name: action[1], params: { projectId: participation.project.id }}"
              >
                <v-list-item-content>
                  <v-list-item-title v-t="action[0]" />
                </v-list-item-content>
              </v-list-item>
            </v-list-group>
          </div>

        </v-list-group>
      </div>

      <!--//* Profile -->
      <v-list-group
        v-if="personModule.getActivePerson"
        prepend-icon="person"
      >
        <template v-slot:activator>
          <v-list-item-content>
            <v-list-item-title v-t="'person.profile'" />
          </v-list-item-content>
        </template>

        <v-list-item
          v-for="(action, i) in profileActions"
          :key="i"
          :to="{ name: action[1] }"
        >
          <v-list-item-icon />
          <v-list-item-content>
            <v-list-item-title v-t="action[0]" />
          </v-list-item-content>
        </v-list-item>
      </v-list-group>

    </v-list>

  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import { mapGetters, mapActions } from 'vuex'
import { Person, Project, Topic, Participation, ParticipationStatus } from '../models/definitions'
import { getModule } from 'vuex-module-decorators'
import PersonModule from '../store/persons'
import KnowledgeBaseModule from '../store/knowledgeBase'

@Component
export default class NavbarSignedIn extends Vue {
  private personModule: PersonModule = getModule(PersonModule, this.$store)
  private knowledgeBaseModule: KnowledgeBaseModule = getModule(KnowledgeBaseModule, this.$store)

  private profileActions: any[] = [
    ['person.personal', 'personal'],
    //// ['Spiritual', 'assignment_turned_in', 'configureProjects'],
    //// administrationAvailability', 'event_available', 'configureProjects'],
    //// ['Notifications', 'notifications', 'configureProjects'],
    ['person.configureProjects', 'configureProjects']
  ]

  private getTopics(project: Project) {
    return this.knowledgeBaseModule.allTopics.filter((topic: Topic) => {
      return topic.projectId === project.id
    })
  }

  private getAdminActions(participation: Participation) {
    const actions = []
    if (participation.role) {
      if (participation.role.settingsRead ||
          participation.role.rolesRead ||
          participation.role.knowledgeBaseWrite) {
        actions.push(['project.settings', 'settings'])
      }
      if (participation.role.participantsRead) {
        actions.push(['project.participants', 'participants'])
      }
      if (participation.role.participantsWrite) {
        actions.push(['project.congregations', 'congregations'])
      }
    }
    return actions
  }

  private canView(participation: Participation, link: string) {
    if (participation.role) {
      if (link === 'calendar') {
        return true
        // TODO: return participation.role.calendarRead
      }
      else if (link === 'topics') {
        return participation.role.knowledgeBaseRead
      }
    }
  }

  private isPendingParticipation(participation: Participation) {
    return participation.status === ParticipationStatus.Invited
      || participation.status === ParticipationStatus.Requested
  }

  private activatePersonIfNotMe(personId: string) {
    if (personId !== this.personModule.getActivePersonId) {
      this.personModule.activatePerson(personId)
    }
  }

}
</script>
