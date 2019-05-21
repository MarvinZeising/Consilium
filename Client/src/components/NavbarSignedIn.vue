<template>
  <div>

    <!--//* Avatar -->
    <v-list-tile avatar>
      <v-list-tile-avatar>
        <img src="https://randomuser.me/api/portraits/men/85.jpg">
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
                Marvin Zeising
                <v-icon right>arrow_drop_down</v-icon>
              </v-btn>
            </template>
            <v-list>
              <v-list-tile
                v-for="(person, i) in myPersons"
                :key="i"
                @click=""
              >
                <v-list-tile-title>
                  <v-icon>person</v-icon>
                  {{ person.firstname }} {{ person.lastname }}
                </v-list-tile-title>
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
      <v-list-tile-title>Home</v-list-tile-title>
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
          <v-list-tile-title v-text="project.name"></v-list-tile-title>
        </v-list-tile>
      </template>

      <v-list-tile
        v-for="(action, j) in project.actions"
        :key="j"
        :to="{ name: action[2], params: { projectId: project.id }}"
      >
        <v-list-tile-action>
          <v-icon v-text="action[1]"></v-icon>
        </v-list-tile-action>
        <v-list-tile-title v-text="action[0]"></v-list-tile-title>
      </v-list-tile>

      <v-list-group
        no-action
        sub-group
      >
        <template v-slot:activator>
          <v-list-tile>
            <v-list-tile-title>Administration</v-list-tile-title>
          </v-list-tile>
        </template>

        <v-list-tile
          v-for="(action, j) in project.adminActions"
          :key="j"
          :to="{ name: action[2], params: { projectId: project.id }}"
        >
          <v-list-tile-action>
            <v-icon v-text="action[1]"></v-icon>
          </v-list-tile-action>
          <v-list-tile-title v-text="action[0]"></v-list-tile-title>
        </v-list-tile>
      </v-list-group>

    </v-list-group>

    <!--//* Profile -->
    <v-list-group
      prepend-icon="person"
    >
      <template v-slot:activator>
        <v-list-tile>
          <v-list-tile-title>Profile</v-list-tile-title>
        </v-list-tile>
      </template>

      <v-list-tile
        v-for="(action, i) in profileActions"
        :key="i"
        :to="{ name: action[2] }"
      >
        <v-list-tile-action>
          <v-icon v-text="action[1]"></v-icon>
        </v-list-tile-action>
        <v-list-tile-title v-text="action[0]"></v-list-tile-title>
      </v-list-tile>
    </v-list-group>

    <!--//* Account -->
    <v-list-tile :to="{ name: 'account' }">
      <v-list-tile-action>
        <v-icon>lock</v-icon>
      </v-list-tile-action>
      <v-list-tile-title>Account</v-list-tile-title>
    </v-list-tile>

    <!--//* Sign out -->
    <v-list-tile :to="{ name: 'signOut' }">
      <v-list-tile-action>
        <v-icon>exit_to_app</v-icon>
      </v-list-tile-action>
      <v-list-tile-title>Sign out</v-list-tile-title>
    </v-list-tile>

  </div>
</template>

<script lang="ts">
import Vue from 'vue'
import axios from 'axios'
import { mapGetters, mapActions } from 'vuex'
import { Person, Project } from '@/models/definitions'
import { getModule } from 'vuex-module-decorators'
import ProjectModule from '@/store/modules/projects'
import UserModule from '@/store/modules/users'
import PersonModule from '@/store/modules/persons';
import Component from 'vue-class-component'

@Component
export default class NavbarSignedIn extends Vue {
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)
  private userModule: UserModule = getModule(UserModule, this.$store)
  private personModule: PersonModule = getModule(PersonModule, this.$store)

  private drawer: boolean = false

  private get myPersons(): Person[] {
    return this.personModule.myPersons
  }

  private profileActions: any[] = [
    ['Personal', 'account_circle', 'configureProjects'],
    ['Spiritual', 'assignment_turned_in', 'configureProjects'],
    ['Availability', 'event_available', 'configureProjects'],
    ['Account', 'lock', 'configureProjects'],
    ['Notifications', 'notifications', 'configureProjects'],
    ['Configure Projects', 'settings', 'configureProjects']
  ]

  private get isSignedIn(): boolean {
    return this.userModule.isSignedIn
  }

  private get myProjects(): Project[] {
    return this.projectModule.myProjects.map((project: any) => {
      project.actions = [
        ['Knowledge Base', 'subject', 'knowledgeBase'],
        ['Calendar', 'today', 'calendar']
      ]
      project.adminActions = [
        ['Settings', 'settings', 'settings'],
        ['User Management', 'group', 'settings'],
        ['Categories', 'label', 'settings'],
        ['Teams', 'supervisor_account', 'settings'],
        ['Meeting Points', 'location_on', 'settings'],
        ['Notifications', 'notifications', 'settings'],
        ['Reports', 'message', 'settings'],
        ['Statistics', 'show_chart', 'settings'],
        ['Notes', 'edit', 'settings']
      ]
      return project
    })
  }

  private async created() {
    await this.personModule.fetchPersons()
  }

}
</script>
