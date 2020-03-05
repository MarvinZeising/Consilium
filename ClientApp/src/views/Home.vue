<template>
  <v-container fluid>
    <v-layout wrap>
      <v-flex v-if="personModule.getActivePerson" xs12 class="mb-5">
        <h1>
          {{ $t('person.home.welcome') }}
          {{ personModule.getActivePerson.firstname }}!
        </h1>
        <p v-t="'person.home.welcomeDescription'" />
      </v-flex>
      <div v-else class="d-flex flex-column" style="width:100%;">
        <h2 v-t="'person.home.noPerson'" class="d-flex justify-center my-6" />
        <div class="d-flex justify-center">
          <v-btn color="primary" :to="{ name: 'account' }">
            {{ $t('person.home.noPersonLink') }}
            <v-icon right>keyboard_arrow_right</v-icon>
          </v-btn>
        </div>
      </div>

      <HomeAttendances v-if="personModule.getActivePerson" />

      <HomeApplications v-if="personModule.getActivePerson" />

      <!-- <h1>Missing Shift Reports</h1>
      <h1>Notifications</h1>
      <h1>Project Enquiries</h1>-->
    </v-layout>
  </v-container>
</template>

<script lang="ts">
import { Vue, Component, Watch } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import UserModule from '../store/users'
import PersonModule from '../store/persons'
import ApplicationModule from '../store/applications'
import ProjectModule from '../store/projects'
import HomeApplications from '../components/HomeApplications.vue'
import HomeAttendances from '../components/HomeAttendances.vue'
import { Project, Topic } from '../models'

@Component({
  components: {
    HomeApplications,
    HomeAttendances,
  },
})
export default class Home extends Vue {
  private userModule = getModule(UserModule, this.$store)
  private personModule = getModule(PersonModule, this.$store)
  private applicationModule = getModule(ApplicationModule, this.$store)
  private projectModule = getModule(ProjectModule, this.$store)
}
</script>
