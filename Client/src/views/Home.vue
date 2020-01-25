<template>
  <v-container fluid>
    <v-layout wrap>

      <v-flex
        xs12
        class="mb-5"
      >
        <h1 v-if="personModule.getActivePerson">
          {{ $t('person.home.welcome') }}
          {{ personModule.getActivePerson.firstname }}!</h1>
        <p v-t="'person.home.welcomeDescription'" />
      </v-flex>

      <HomeApplications />

      <!-- <h1>Missing Shift Reports</h1>
      <h1>Upcoming Shifts</h1>
      <h1>Notifications</h1>
      <h1>Project Enquiries</h1> -->

    </v-layout>
  </v-container>
</template>

<script lang="ts">
import { Vue, Component, Watch } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import PersonModule from '../store/persons'
import ApplicationModule from '../store/applications'
import ProjectModule from '../store/projects'
import HomeApplications from '../components/HomeApplications.vue'
import { Project, Topic } from '../models'

@Component({
  components: {
    HomeApplications,
  }
})
export default class Home extends Vue {
  private personModule = getModule(PersonModule, this.$store)
  private applicationModule = getModule(ApplicationModule, this.$store)
  private projectModule = getModule(ProjectModule, this.$store)

}
</script>
