<template>
  <v-dialog
    v-model="dialog"
    max-width="400px"
  >
    <template v-slot:activator="{ on }">
      <v-btn
        v-on="on"
        text
        v-t="'project.participant.filter'"
      />
    </template>
    <v-card>
      <v-form ref="form">
        <v-card-text>

          <p v-t="'project.participant.searchDescription'" />
          <v-text-field
            v-if="filter"
            v-model="filter.search"
            :label="$t('core.search')"
            filled
          />

        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn
            text
            v-t="'core.close'"
            @click.stop="dialog = false"
          />
        </v-card-actions>
      </v-form>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'

@Component
export default class FilterParticipantsDialog extends Vue {
  private dialog: any = null
  private valid = false

  @Prop(Object)
  private readonly filter?: { search: string }

}
</script>
