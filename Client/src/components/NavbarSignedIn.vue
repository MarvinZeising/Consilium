<template>
  <div>

    <!--//* Avatar -->
    <v-list-tile avatar>
      <v-list-tile-avatar>
        <img
          v-if="getActivePerson.photoUrl"
          :src="getActivePerson.photoUrl"
        />
        <img
          v-if="!getActivePerson.photoUrl"
          src="../assets/person.jpg"
        />
      </v-list-tile-avatar>
      <v-list-tile-content>
        <v-menu
          transition="slide-y-transition"
          bottom
        >
          <template v-slot:activator="{ on }">
            <v-btn
              outline
              class="grey text--black"
              v-on="on"
            >
              {{ getActivePerson.fullName() }}
              <v-icon right>arrow_drop_down</v-icon>
            </v-btn>
          </template>
          <v-list>
            <v-list-tile
              v-for="(person, i) in persons"
              :key="i"
              @click="activatePerson(person.id)"
            >
              <v-list-tile-title>
                <v-icon left>person</v-icon>
                <span>{{person.fullName()}}</span>
              </v-list-tile-title>
            </v-list-tile>

            <v-list-tile :to="{ name: 'createPerson' }">
              <v-icon left>person_add</v-icon>
              {{ $t('navbar.createPerson') }}
            </v-list-tile>
          </v-list>
        </v-menu>
      </v-list-tile-content>
    </v-list-tile>

    <!--//* Home -->
    <v-list-tile :to="{ name: 'home' }">
      <v-list-tile-action>
        <v-icon>home</v-icon>
      </v-list-tile-action>
      <v-list-tile-title v-t="'navbar.home'"/>
    </v-list-tile>

    <!--//* Projects -->
    <v-list-group
      v-for="(project, i) in myProjects"
      :key="i"
      prepend-icon="extension"
      value="true"
    >
      <template v-slot:activator>
        <v-list-tile>
          <v-list-tile-title v-text="project.name" />
        </v-list-tile>
      </template>

      <v-list-tile :to="{ name: 'calendar', params: { projectId: project.id }}">
        <v-list-tile-action>
          <v-icon>today</v-icon>
        </v-list-tile-action>
        <v-list-tile-title v-t="'navbar.calendar'" />
      </v-list-tile>

      <v-list-group
        v-if="project.topics.length > 0"
        no-action
        sub-group
      >
        <template v-slot:activator>
          <v-list-tile>
            <v-list-tile-title v-t="'navbar.knowledgeBase'" />
          </v-list-tile>
        </template>

        <v-list-tile
          v-for="(topic, j) in project.topics"
          :key="j"
          :to="{ name: 'knowledgeBase', params: { projectId: project.id, topicId: topic.id }}"
        >
          <v-list-tile-title v-text="topic.name" />
        </v-list-tile>
      </v-list-group>

      <v-list-group
        no-action
        sub-group
      >
        <template v-slot:activator>
          <v-list-tile>
            <v-list-tile-title v-t="'navbar.administration'" />
          </v-list-tile>
        </template>

        <v-list-tile
          v-for="(action, j) in project.adminActions"
          :key="j"
          :to="{ name: action[2], params: { projectId: project.id }}"
        >
          <v-list-tile-action>
            <v-icon v-text="action[1]" />
          </v-list-tile-action>
          <v-list-tile-title v-t="action[0]" />
        </v-list-tile>
      </v-list-group>

    </v-list-group>

    <!--//* Profile -->
    <v-list-group prepend-icon="person">
      <template v-slot:activator>
        <v-list-tile>
          <v-list-tile-title v-t="'navbar.profile'" />
        </v-list-tile>
      </template>

      <v-list-tile
        v-for="(action, i) in profileActions"
        :key="i"
        :to="{ name: action[2] }"
      >
        <v-list-tile-action>
          <v-icon v-text="action[1]" />
        </v-list-tile-action>
        <v-list-tile-title v-t="action[0]" />
      </v-list-tile>
    </v-list-group>

    <!--//* Account -->
    <v-list-tile :to="{ name: 'account' }">
      <v-list-tile-action>
        <v-icon>lock</v-icon>
      </v-list-tile-action>
      <v-list-tile-title v-t="'navbar.account'" />
    </v-list-tile>

    <!--//* Sign out -->
    <v-list-tile :to="{ name: 'signOut' }">
      <v-list-tile-action>
        <v-icon>exit_to_app</v-icon>
      </v-list-tile-action>
      <v-list-tile-title v-t="'navbar.signOut'" />
    </v-list-tile>

  </div>
</template>

<script lang="ts">
import Vue from 'vue'
import { mapGetters, mapActions } from 'vuex'
import { Person, Project, Topic } from '@/models/definitions'
import { getModule } from 'vuex-module-decorators'
import UserModule from '@/store/modules/users'
import PersonModule from '@/store/modules/persons'
import ProjectModule from '@/store/modules/projects'
import KnowledgeBaseModule from '../store/modules/knowledgeBase'
import Component from 'vue-class-component'

@Component
export default class NavbarSignedIn extends Vue {
  private userModule: UserModule = getModule(UserModule, this.$store)
  private personModule: PersonModule = getModule(PersonModule, this.$store)
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)
  private knowledgeBaseModule: KnowledgeBaseModule = getModule(KnowledgeBaseModule, this.$store)

  private drawer: boolean = false

  private get persons(): any[] {
    return this.personModule.myPersons
  }

  private get getActivePerson(): Person {
    return this.personModule.getActivePerson
  }

  private profileActions: any[] = [
    ['navbar.personal', 'account_circle', 'personal'],
    //// ['Spiritual', 'assignment_turned_in', 'configureProjects'],
    //// administrationAvailability', 'event_available', 'configureProjects'],
    //// ['Account', 'lock', 'configureProjects'],
    //// ['Notifications', 'notifications', 'configureProjects'],
    ['navbar.configureProjects', 'settings', 'configureProjects']
  ]

  private get isSignedIn(): boolean {
    return this.userModule.isSignedIn
  }

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

  private async activatePerson(personId: string) {
    await this.personModule.activatePerson(personId)
  }

}
</script>
