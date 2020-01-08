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
                {{ personModule.getActivePerson.fullName() }}
              </v-list-item-title>
              <v-list-item-subtitle>Wuppertal-Nord</v-list-item-subtitle>
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
            <span>{{ person.fullName() }}</span>
          </v-list-item-title>
        </v-list-item>
      </v-list>
    </v-menu>
    <v-list v-else-if="personModule.getActivePerson">
      <v-list-item two-line>
        <v-list-item-content>
          <v-list-item-title class="title">
            {{ personModule.getActivePerson.fullName() }}
          </v-list-item-title>
          <v-list-item-subtitle>Wuppertal-Nord</v-list-item-subtitle>
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

      <div v-if="personModule.getActivePerson">
        <!--//* Projects -->
        <v-list-group
          v-for="(participation, i) in personModule.getActivePerson.getParticipations"
          :key="i"
          prepend-icon="extension"
        >
          <template v-slot:activator>
            <v-list-item-content>
              <v-list-item-title v-text="participation.project.name" />
            </v-list-item-content>
          </template>

          <v-list-item :to="{ name: 'calendar', params: { projectId: participation.project.id }}">
            <v-list-item-icon />
            <v-list-item-content>
              <v-list-item-title v-t="'project.calendar'" />
            </v-list-item-content>
          </v-list-item>

          <v-list-group
            v-if="getTopics(participation.project).length > 0"
            no-action
            sub-group
          >
            <template v-slot:activator>
              <v-list-item-content>
                <v-list-item-title v-t="'project.knowledgeBase'" />
              </v-list-item-content>
            </template>

            <v-list-item
              v-for="(topic, j) in getTopics(participation.project)"
              :key="j"
              :to="{ name: 'topic', params: { projectId: participation.project.id, topicId: topic.id }}"
            >
              <v-list-item-content>
                <v-list-item-title v-text="topic.name" />
              </v-list-item-content>
            </v-list-item>
          </v-list-group>

          <v-list-group
            no-action
            sub-group
          >
            <template v-slot:activator>
              <v-list-item-content>
                <v-list-item-title v-t="'project.administration'" />
              </v-list-item-content>
            </template>

            <v-list-item
              v-for="(action, j) in getAdminActions(participation.project)"
              :key="j"
              :to="{ name: action[1], params: { projectId: participation.project.id }}"
            >
              <v-list-item-content>
                <v-list-item-title v-t="action[0]" />
              </v-list-item-content>
            </v-list-item>
          </v-list-group>

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
import { Person, Project, Topic, Participation } from '../models/definitions'
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

  private getAdminActions() {
    // TODO: do some permission checks and only display the ones one has access to
    return [
      ['project.settings', 'settings'],
      ['project.participants', 'participants'],
      //// ['Categories', 'label', 'settings'],
      //// ['Teams', 'supervisor_account', 'settings'],
      //// ['Meeting Points', 'location_on', 'settings'],
      //// ['Notifications', 'notifications', 'settings'],
      //// ['Reports', 'message', 'settings'],
      //// ['Statistics', 'show_chart', 'settings'],
      //// ['Notes', 'edit', 'settings']
    ]
  }

  private activatePersonIfNotMe(personId: string) {
    if (personId !== this.personModule.getActivePersonId) {
      this.personModule.activatePerson(personId)
    }
  }

}
</script>
