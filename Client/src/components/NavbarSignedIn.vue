<template>
  <div>

    <!-- //* Person -->
    <v-menu
      v-if="personModule.getPersons.length > 1"
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
          @click="personModule.activatePerson(person.id)"
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
          <v-list-item-title v-t="'navbar.home'"/>
        </v-list-item-content>
      </v-list-item>

      <!--//* Projects -->
      <v-list-group
        v-for="(project, i) in myProjects"
        :key="i"
        prepend-icon="extension"
        value="true"
      >
        <template v-slot:activator>
          <v-list-item-content>
            <v-list-item-title v-text="project.name" />
          </v-list-item-content>
        </template>

        <v-list-item :to="{ name: 'calendar', params: { projectId: project.id }}">
          <v-list-item-icon />
          <v-list-item-content>
            <v-list-item-title v-t="'navbar.calendar'" />
          </v-list-item-content>
        </v-list-item>

        <v-list-group
          v-if="project.topics.length > 0"
          no-action
          sub-group
        >
          <template v-slot:activator>
            <v-list-item-content>
              <v-list-item-title v-t="'navbar.knowledgeBase'" />
            </v-list-item-content>
          </template>

          <v-list-item
            v-for="(topic, j) in project.topics"
            :key="j"
            :to="{ name: 'topic', params: { projectId: project.id, topicId: topic.id }}"
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
              <v-list-item-title v-t="'navbar.administration'" />
            </v-list-item-content>
          </template>

          <v-list-item
            v-for="(action, j) in project.adminActions"
            :key="j"
            :to="{ name: action[2], params: { projectId: project.id }}"
          >
            <v-list-item-content>
              <v-list-item-title v-t="action[0]" />
            </v-list-item-content>
          </v-list-item>
        </v-list-group>

      </v-list-group>

      <!--//* Profile -->
      <v-list-group
        v-if="personModule.getActivePerson"
        prepend-icon="person"
      >
        <template v-slot:activator>
          <v-list-item-content>
            <v-list-item-title v-t="'navbar.profile'" />
          </v-list-item-content>
        </template>

        <v-list-item
          v-for="(action, i) in profileActions"
          :key="i"
          :to="{ name: action[2] }"
        >
          <v-list-item-icon>
            <v-icon v-text="action[1]" />
          </v-list-item-icon>
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
import { Person, Project, Topic } from '@/models/definitions'
import { getModule } from 'vuex-module-decorators'
import UserModule from '@/store/modules/users'
import PersonModule from '@/store/modules/persons'
import ProjectModule from '@/store/modules/projects'
import KnowledgeBaseModule from '../store/modules/knowledgeBase'

@Component
export default class NavbarSignedIn extends Vue {
  private userModule: UserModule = getModule(UserModule, this.$store)
  private personModule: PersonModule = getModule(PersonModule, this.$store)
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)
  private knowledgeBaseModule: KnowledgeBaseModule = getModule(KnowledgeBaseModule, this.$store)

  private profileActions: any[] = [
    ['navbar.personal', 'account_circle', 'personal'],
    //// ['Spiritual', 'assignment_turned_in', 'configureProjects'],
    //// administrationAvailability', 'event_available', 'configureProjects'],
    //// ['Account', 'lock', 'configureProjects'],
    //// ['Notifications', 'notifications', 'configureProjects'],
    ['navbar.configureProjects', 'settings', 'configureProjects']
  ]

  private get myProjects(): Project[] {
    return this.projectModule.myProjects.map((project: any) => {
      project.topics = this.knowledgeBaseModule.allTopics.filter((topic: Topic) => {
        return topic.projectId === project.id
      })
      project.adminActions = [
        ['navbar.settings', 'settings', 'settings'],
        //// ['User Management', 'group', 'settings'],
        //// ['Categories', 'label', 'settings'],
        //// ['Teams', 'supervisor_account', 'settings'],
        //// ['Meeting Points', 'location_on', 'settings'],
        //// ['Notifications', 'notifications', 'settings'],
        //// ['Reports', 'message', 'settings'],
        //// ['Statistics', 'show_chart', 'settings'],
        //// ['Notes', 'edit', 'settings']
      ]
      return project
    })
  }

}
</script>
