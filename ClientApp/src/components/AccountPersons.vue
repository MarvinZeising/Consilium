<template>
  <v-flex xs12 sm10 md8 lg6 xl4>
    <v-card flat class="ma-2 mb-5">
      <v-card-text class="text--primary">
        <h2 class="headline">{{ $tc('person.persons', 2) }}</h2>
        <span class="grey--text">{{ $t('person.personsDescription') }}</span>
      </v-card-text>

      <v-list>
        <v-list-item
          v-if="personModule.getPersons.length === 0"
          class="accent"
          dark
        >{{ $tc('person.persons', 0) }}</v-list-item>

        <v-list-item v-for="(person, index) in personModule.getPersons" :key="index">
          <v-list-item-content>
            <v-list-item-title v-text="person.getFullName" />
            <v-list-item-subtitle v-if="person.congregation" v-text="person.congregation.name" />
          </v-list-item-content>

          <v-list-item-action>
            <v-btn icon class="ma-0" @click.stop="editPerson(person.id)">
              <v-icon color="grey">arrow_forward</v-icon>
            </v-btn>
          </v-list-item-action>
        </v-list-item>
      </v-list>

      <v-card-actions>
        <v-spacer />
        <CreatePersonDialog />
      </v-card-actions>
    </v-card>
  </v-flex>
</template>

<script lang="ts">
import { Vue, Component, Watch } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import PersonModule from '../store/persons'
import UserModule from '../store/users'
import { Person } from '../models'
import CreatePersonDialog from './dialogs/CreatePersonDialog.vue'

@Component({
  components: {
    CreatePersonDialog,
  },
})
export default class AccountPersons extends Vue {
  private personModule = getModule(PersonModule, this.$store)

  private editPerson(personId: string) {
    if (this.personModule.getActivePersonId !== personId) {
      this.personModule.activatePerson(personId)
    }
    this.$router.push({ name: 'personal' })
  }
}
</script>
