<template>
  <v-dialog v-model="renameTopicDialog" persistent max-width="600px">
    <template v-slot:activator="{ on }">
      <v-btn
        v-on="on"
        icon
        class="ma-0"
      >
        <v-icon color="grey">edit</v-icon>
      </v-btn>
    </template>
    <v-card>
      <v-form v-model="form">
        <v-card-title>
          <span
            class="headline"
            v-t="'knowledgeBase.renameTopic'"
          ></span>
        </v-card-title>
        <v-card-text>
          <v-text-field
            v-model="topicName"
            :label="$t('core.name')"
            filled
            required
          />
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn
            text
            @click="renameTopicDialog = false"
            v-t="'core.close'"
          />
          <v-btn
            :disabled="topicName == ''"
            text
            color="primary"
            @click="renameTopic"
            v-t="'core.save'"
            type="submit"
          />
        </v-card-actions>
      </v-form>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import KnowledgeBaseModule from '@/store/modules/knowledgeBase'
import { getModule } from 'vuex-module-decorators'
import { Vue, Component, Prop } from 'vue-property-decorator'
import { Topic } from '../../models/definitions'
import i18n from '@/i18n'

@Component
export default class RenameTopicDialog extends Vue {
  private knowledgeBaseModule: KnowledgeBaseModule = getModule(KnowledgeBaseModule, this.$store)

  @Prop(String)
  private readonly topicId: string | undefined

  private form: any = null
  private renameTopicDialog: any = null
  private topicName: string = ''

  private async created() {
    this.topicName = await this.knowledgeBaseModule.allTopics
      .filter((x: Topic) => x.id === this.topicId)[0].name
  }

  private async renameTopic() {
    const updatedTopic = new Topic('', this.topicName)
    updatedTopic.id = this.topicId || ''

    await this.knowledgeBaseModule.changeTopic(updatedTopic)

    this.renameTopicDialog = false
  }
}
</script>
